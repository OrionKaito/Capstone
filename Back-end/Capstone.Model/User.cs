using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string EmailConfirmCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreateDate { get; set; }
        public string ImagePath { get; set; }

        public string LineManagerID { get; set; }
        [ForeignKey("LineManagerID")]
        public virtual User LineManager { get; set; }

        public bool IsDeleted { get; set; }
    }
}
