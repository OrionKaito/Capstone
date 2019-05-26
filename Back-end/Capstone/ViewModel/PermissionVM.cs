using System;

namespace Capstone.ViewModel
{
    public class PermissionVM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class PermissionCM
    {
        public string Name { get; set; }
    }

    public class PermissionUM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
