using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModel
{
  
    public class RequestVM
    {
        public Guid ID { get; set; }
        public DateTime timeStamp { get; set; }
    }

    public class RequestCM
    {
        public DateTime timeStamp { get; set; }
    }

    public class RequestUM
    {
        public Guid ID { get; set; }
        public DateTime timeStamp { get; set; }
    }
}
