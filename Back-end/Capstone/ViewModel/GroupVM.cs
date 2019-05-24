using System;

namespace Capstone.ViewModel
{
    public class GroupVM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class GroupCM
    {
        public string Name { get; set; }
    }

    public class GroupUM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
