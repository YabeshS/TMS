using DTO;
using Microsoft.EntityFrameworkCore;
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

namespace Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<MUser> _user;
        private readonly IConfiguration _configuration;
        private readonly IUserContextService _usercontextservice;
        private int userid;
        public AuthService(IGenericRepository<MUser> user, IConfiguration configuration, IUserContextService usercontextservice)
        {
            _user = user;
            _configuration = configuration;
            _usercontextservice = usercontextservice;
            userid= _usercontextservice.GetUserId();
        }
        public async Task<string> LoginAsync(LoginDto logindto)
        {
            try
            {
                if (logindto.EmailId == null || logindto.Password == null)
                {
                    throw new Exception("Invalid email or password");
                }
                var users = await _user.GetAllAsync(x => x.Email == logindto.EmailId && (x.IsActive == true),include:q=>q.Include(u=>u.Role));
                var user = users.FirstOrDefault();
                if (user == null)
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
            //new Claim("RoleId", user.RoleId.ToString()),
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
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }
        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                if (await _user.AnyAsync(x => x.Email == registerDto.EmailId))
                    throw new Exception("Email already exists");
                var exist = await _user.AnyAsync(x => x.FullName == registerDto.Name && x.RoleId == registerDto.RoleId);
                if (exist) throw new Exception("User already exists");

                var hashedpassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                var user = new MUser
                {
                    FullName = registerDto.Name,
                    Email = registerDto.EmailId,
                    PasswordHash = hashedpassword,
                    RoleId = registerDto.RoleId,
                    IsActive = true,
                    Createdby = userid
                };
                await _user.AddAsync(user);
                return "User registered successfully";
            }
            catch (Exception ex) 
            { throw new Exception(ex.Message); }
        }
        public async Task<List<GetAllUsersDto>> GetAllUserAsync()
        {
            try
            {
                var users = await _user.GetAllAsync();
                //var userslist = users.ToList();
                //var usersinfo = new GetAllUsersDto
                //{
                //    Userid=users.u
                //};
                var response =  users.Select(u => new GetAllUsersDto
                { Userid=u.Userid,
                FullName=u.FullName,
                Email=u.Email,
                RoleId=u.RoleId,
                IsActive=u.IsActive,
                Createdby=u.Createdby,
                CreatedDate=u.CreatedDate,
                Updatedby=u.Updatedby,
                UpdatedDate=u.UpdatedDate
                }).ToList();
                return response;
            } catch (Exception ex) { throw new Exception(ex.Message); } }
    }

}