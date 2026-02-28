using DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TMS.Repository;

namespace Service
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<MUser> _user;
        private readonly IConfiguration _configuration;
        public AuthService(IGenericRepository<MUser> user, IConfiguration configuration)
        {
            _user = user;
            _configuration = configuration;
        }
        public async Task<string> LoginAsync(LoginDto logindto) 
        {
            if(logindto.EmailId == null || logindto.Password == null)
            {
                throw new Exception ("Invalid email or password");
            }
            var user = await _user.FirstOrDefaultAsync(x => x.Email == logindto.EmailId && (x.IsActive==true));
            if(user== null)
            {
                throw new Exception("User not found");
            }
            bool isValid = BCrypt.Net.BCrypt.Verify(logindto.Password, user.PasswordHash);
            if (!isValid)
                throw new Exception("Invalid password");

            var claims = new[]
                    {
            new Claim(ClaimTypes.NameIdentifier, user.Userid.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleName)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}