using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Repository.Models;
namespace Service
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto logindto);
        Task<string> RegisterAsync(RegisterDto regosterdto);
        Task<List<GetAllUsersDto>> GetAllUserAsync();
    }
}
