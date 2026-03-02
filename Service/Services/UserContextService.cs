using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserContextService: IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContextService(IHttpContextAccessor httpContextAccessor) 
        { 
            _httpContextAccessor = httpContextAccessor; 
        }
        public int GetUserId() {
            var userId = _httpContextAccessor.HttpContext?.User
            ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId != null ? int.Parse(userId) : 0;
        }
        public string GetCurrentUserRole()
        {
            return _httpContextAccessor.HttpContext?.User
                ?.FindFirst(ClaimTypes.Role)?.Value;
        }

    }
}
