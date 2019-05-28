using System;

namespace Capstone.ViewModel
{
    public class PermissionOfRoleVM
    {
        public Guid ID { get; set; }
        public Guid PermissionID { get; set; }
        public Guid RoleID { get; set; }
    }

    public class PermissionOfRoleCM
    {
        public Guid PermissionID { get; set; }
        public Guid RoleID { get; set; }
    }

    public class PermissionOfRoleUM
    {
        public Guid ID { get; set; }
        public Guid PermissionID { get; set; }
        public Guid RoleID { get; set; }
    }
}
