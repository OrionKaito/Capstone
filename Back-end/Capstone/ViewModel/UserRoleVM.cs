using System;

namespace Capstone.ViewModel
{
    public class UserRoleVM
    {
        public Guid ID { get; set; }
        public string UserID { get; set; }
        public string FullName { get; set; }
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
    }

    public class UserRoleCM
    {
        public string UserID { get; set; }
        public Guid RoleID { get; set; }
    }

    public class UserRoleUM
    {
        public Guid ID { get; set; }
        public string UserID { get; set; }
        public Guid RoleID { get; set; }
    }
}
