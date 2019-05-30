using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModel
{
 
    public class RequestActionVM
    {
        public Guid ID { get; set; }
        public String status { get; set; }
        public DateTime timeStamp { get; set; }
    }

    public class RequestActionCM
    {
        public String status { get; set; }
        public DateTime timeStamp { get; set; }
    }

    public class RequestActionUM
    {
        public Guid ID { get; set; }
        public String status { get; set; }
        public DateTime timeStamp { get; set; }
    }
}
