using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
