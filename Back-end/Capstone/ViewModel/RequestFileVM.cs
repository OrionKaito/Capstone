using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.ViewModel
{

    public class RequestFileVM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid RequestActionID { get; set; }
    }

    public class RequestFileCM
    {
        public string Name { get; set; }
        public Guid RequestActionID { get; set; }
    }

    public class RequestFileUM
    {
        public Guid ID { get; set; }
    }
}
