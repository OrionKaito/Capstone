using System;

namespace Capstone.ViewModel
{
    public class RoleVM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class RoleCM
    {
        public string Name { get; set; }
    }

    public class RoleUM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
