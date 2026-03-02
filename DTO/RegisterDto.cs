using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RegisterDto
    { 
        public string Name { get; set; }
        public string EmailId{get;set;}
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
    public class GetAllUsersDto
    {
        public int Userid { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

    }
}
