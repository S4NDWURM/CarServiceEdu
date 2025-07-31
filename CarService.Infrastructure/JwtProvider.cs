using CarService.Core.Models;
using CarService.DataAccess.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Infrastructure
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;
        private readonly IRoleRepository _roleRepository;
        public JwtProvider(IOptions<JwtOptions> options, IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
            _options = options.Value;
        }

        public string GenerateToken(User user)
        {
            var role = _roleRepository
                .GetById(user.RoleId)
                .GetAwaiter()
                .GetResult();

            if (role is null)
                throw new InvalidOperationException($"Role id {user.RoleId} not found");

            Claim[] claims = [
                new("userId", user.Id.ToString()),
                new Claim("role", role.Name)
                ];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours)
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
