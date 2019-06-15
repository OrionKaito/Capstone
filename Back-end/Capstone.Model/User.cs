using Microsoft.AspNetCore.Identity;
using System;

namespace Capstone.Model
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string EmailConfirmCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? ManagerID { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
