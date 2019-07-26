using System;
using System.Collections.Generic;

namespace Capstone.ViewModel
{
    public class PermissionOfGroupVM
    {
        public Guid ID { get; set; }
        public Guid PermissionID { get; set; }
        public string PermissionName { get; set; }
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
    }

    public class PermissionOfGroupCM
    {
        public Guid PermissionID { get; set; }
        public Guid GroupID { get; set; }
    }

    public class PermissionOfGroupUM
    {
        public Guid ID { get; set; }
        public Guid PermissionID { get; set; }
        public Guid GroupID { get; set; }
    }

    public class PermissionOfGroupViewModel
    {
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<PermissionsViewModel> Permissions { get; set; }
    }

    public class PermissionsViewModel
    {
        public Guid PermissionOfGroupID { get; set; }
        public Guid PermissionID { get; set; }
        public string PermissionName { get; set; }
    }
}
