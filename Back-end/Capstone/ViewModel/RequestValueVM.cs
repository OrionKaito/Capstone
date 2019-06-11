using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModel
{

    public class RequestValueVM
    {
        public Guid ID { get; set; }
        public string Data { get; set; }
        public Guid RequestActionID { get; set; }
    }

    public class RequestValueCM
    {
        public string Data { get; set; }
    }

    public class RequestValueUM
    {
        public Guid ID { get; set; }
        public string Data { get; set; }
        public Guid RequestActionID { get; set; }
    }
}
