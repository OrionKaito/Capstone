using System;

namespace Capstone.ViewModel
{
    public class UserGroupVM
    {
        public Guid ID { get; set; }
        public string UserId { get; set; }
        public Guid GroupID { get; set; }
    }

    public class UserGroupCM
    {
        public string UserId { get; set; }
        public Guid GroupID { get; set; }
    }

    public class UserGroupUM
    {
        public Guid ID { get; set; }
        public string UserId { get; set; }
        public Guid GroupID { get; set; }
    }
}
