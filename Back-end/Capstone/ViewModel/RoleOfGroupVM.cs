using System;

namespace Capstone.ViewModel
{
    public class RoleOfGroupVM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid RoleID { get; set; }
        public Guid GroupID { get; set; }
    }

    public class RoleOfGroupCM
    {
        public string Name { get; set; }
        public Guid RoleID { get; set; }
        public Guid GroupID { get; set; }
    }

    public class RoleOfGroupUM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid RoleID { get; set; }
        public Guid GroupID { get; set; }
    }
}
