using System.Net;
using System.Text.Json;
using Npgsql;                 
using Microsoft.EntityFrameworkCore; 

namespace CarService.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            object payload = new { error = "An unexpected error occurred." };

            if (exception is DbUpdateException dbUpdateEx)
            {
                if (dbUpdateEx.InnerException is PostgresException pgInner)
                {
                    return HandlePostgresException(context, pgInner);
                }

                statusCode = HttpStatusCode.BadRequest;
                payload = new { error = dbUpdateEx.Message };
            }
            else if (exception is PostgresException pgEx)
            {
                return HandlePostgresException(context, pgEx);
            }
            else
            {
                switch (exception)
                {
                    case KeyNotFoundException _:
                        statusCode = HttpStatusCode.NotFound;
                        payload = new { error = exception.Message };
                        break;

                    case UnauthorizedAccessException _:
                        statusCode = HttpStatusCode.Unauthorized;
                        payload = new { error = exception.Message };
                        break;

                    case InvalidOperationException _:
                        statusCode = HttpStatusCode.BadRequest;
                        payload = new { error = exception.Message };
                        break;

                    case ConflictException _:
                        statusCode = HttpStatusCode.Conflict;
                        payload = new { error = exception.Message };
                        break;
                    case ArgumentException _:
                        statusCode = HttpStatusCode.BadRequest;
                        payload = new { error = exception.Message };
                        break;
                }
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            var json = JsonSerializer.Serialize(payload);
            return context.Response.WriteAsync(json);
        }
        private static Task HandlePostgresException(HttpContext context, PostgresException pgEx)
        {
            HttpStatusCode status;
            object payload;

            // 23505 — unique_violation
            // 23503 — foreign_key_violation
            // 23502 — not_null_violation
            // 23514 — check_violation
            // 23503 — foreign_key_violation
            // 22001 — string_data_right_truncation
            switch (pgEx.SqlState)
            {
                case "23505":
                    status = HttpStatusCode.Conflict;
                    payload = new { error = "Conflict: duplicate key violation." };
                    break;

                case "23503":
                    status = HttpStatusCode.BadRequest;
                    payload = new { error = "Foreign key violation: The record cannot be deleted or updated because it is referenced by another entity." };
                    break;

                case "23502":
                    status = HttpStatusCode.BadRequest;
                    payload = new { error = "Null value in column that does not allow nulls." };
                    break;

                case "23514":
                    status = HttpStatusCode.BadRequest;
                    payload = new { error = "Check constraint violation." };
                    break;

                case "22001":
                    status = HttpStatusCode.BadRequest;
                    payload = new { error = "Value is too long for column." };
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    payload = new { error = "Database error occurred." };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            var json = JsonSerializer.Serialize(payload);
            return context.Response.WriteAsync(json);
        }
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
