using System;

namespace Capstone.ViewModel
{
    public class UserRoleVM
    {
        public Guid ID { get; set; }
        public string UserId { get; set; }
        public Guid RoleID { get; set; }
    }

    public class UserRoleCM
    {
        public string UserId { get; set; }
        public Guid RoleID { get; set; }
    }

    public class UserRoleUM
    {
        public Guid ID { get; set; }
        public string UserId { get; set; }
        public Guid RoleID { get; set; }
    }
}
