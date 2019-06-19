using Capstone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModel
{
    public class ConnectionTypeVM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public StatusEnum Status { get; set; }
    }

    public class ConnectionTypeCM
    {
        public string Name { get; set; }
        public StatusEnum Status { get; set; }
    }

    public class ConnectionTypeUM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public StatusEnum Status { get; set; }
    }
}
