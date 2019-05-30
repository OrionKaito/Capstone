using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModel
{

    public class RequestValueVM
    {
        public Guid ID { get; set; }
        public String data { get; set; }
        public DateTime timeStamp { get; set; }
    }

    public class RequestValueCM
    {
        public String data { get; set; }
        public DateTime timeStamp { get; set; }
    }

    public class RequestValueUM
    {
        public Guid ID { get; set; }
        public String data { get; set; }
        public DateTime timeStamp { get; set; }
    }
}
