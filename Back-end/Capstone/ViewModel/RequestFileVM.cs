using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModel
{

    public class RequestFileVM
    {
        public Guid ID { get; set; }
        public String name { get; set; }
        public String path { get; set; }
        public DateTime timeStamp { get; set; }
        public bool isEnable { get; set; }

    }

    public class RequestFileCM
    {
        public String name { get; set; }
        public String path { get; set; }
        public DateTime timeStamp { get; set; }
        public bool isEnable { get; set; }
    }

    public class RequestFileUM
    {
        public Guid ID { get; set; }
        public String name { get; set; }
        public String path { get; set; }
        public DateTime timeStamp { get; set; }
        public bool isEnable { get; set; }
    }
}
