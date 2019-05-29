using Microsoft.AspNetCore.Identity;
using System;

namespace Capstone.Model
{
    public class User : IdentityUser
    {
        public String FullName { get; set; }
        public bool? IsEnabled { get; set; }
        
    }
}
