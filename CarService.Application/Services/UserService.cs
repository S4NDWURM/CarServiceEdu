using CarService.Core.Models;
using CarService.DataAccess;
using CarService.DataAccess.Repositories;
using CarService.Infrastructure;

namespace CarService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly CarServiceDbContext _dbContext;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider, CarServiceDbContext dbContext, IClientRepository clientRepository, IEmployeeRepository employeeRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _dbContext = dbContext;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Guid> GetEmployeeIdByUserId(Guid userId)
        {
            var user = await _userRepository.GetByUserId(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (!user.EmployeeId.HasValue)
            {
                throw new InvalidOperationException("EmployeeId is null for the user");
            }

            return user.EmployeeId.Value;
        }

        public async Task<Guid> GetClientIdByUserId(Guid userId)
        {
            var user = await _userRepository.GetByUserId(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (!user.ClientId.HasValue)
            {
                throw new InvalidOperationException("ClientId is null for the user");
            }

            return user.ClientId.Value; 
        }


        public async Task RegisterClient(string userName, string email, string password, Guid roleId, string lastName, string firstName, string middleName, DateTime dateOfBirth, DateTime registrationDate)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var (client, clientError) = Client.Create(Guid.NewGuid(), lastName, firstName, middleName, dateOfBirth, registrationDate);
                if (clientError != string.Empty)
                {
                    throw new ArgumentException(clientError);
                }
                await _clientRepository.Create(client);

                var hashedPassword = _passwordHasher.Generate(password);

                var (user, error) = User.Create(Guid.NewGuid(), userName, hashedPassword, email, roleId, client.Id, null);
                if (error != string.Empty)
                {
                    throw new ArgumentException(error);
                }
                await _userRepository.Add(user);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception rollbackEx)
                {
                    throw new InvalidOperationException("Rollback err", rollbackEx);
                }

                throw;
            }


        }

        public async Task RegisterEmployee(string userName, string email, string password, Guid roleId,
            Guid employeeStatusId, string lastName, string firstName, string middleName, int workExperience, DateTime hireDate)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var (employee, employeeError) = Employee.Create(Guid.NewGuid(), lastName, firstName, middleName, workExperience, hireDate, employeeStatusId);
                if (employeeError != string.Empty)
                {
                    throw new ArgumentException(employeeError);
                }
                await _employeeRepository.Create(employee);

                var hashedPassword = _passwordHasher.Generate(password);

                var (user, error) = User.Create(Guid.NewGuid(), userName, hashedPassword, email, roleId, null, employee.Id);
                if (error != string.Empty)
                {
                    throw new ArgumentException(error);
                }
                await _userRepository.Add(user);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception rollbackEx)
                {
                    throw new InvalidOperationException("Rollback err", rollbackEx);
                }

                throw;
            }


        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                throw new KeyNotFoundException($"Пользователь с email '{email}' не найден.");
            }

            var isPasswordCorrect = _passwordHasher.Verify(password, user.PasswordHash);
            if (!isPasswordCorrect)
            {
                throw new UnauthorizedAccessException("Неверный пароль.");
            }

            return _jwtProvider.GenerateToken(user);
        }
    }
}
