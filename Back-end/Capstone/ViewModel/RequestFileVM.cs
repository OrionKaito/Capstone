using System;

namespace Capstone.ViewModel
{

    public class RequestFileVM
    {

        public Guid ID { get; set; }
        public string Path { get; set; }
        public bool IsDeleted { get; set; }
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
