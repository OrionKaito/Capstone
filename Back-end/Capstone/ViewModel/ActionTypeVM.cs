using System;

namespace Capstone.ViewModel
{
    public class ActionTypeVM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }

    public class ActionTypeCM
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }

    public class ActionTypeUM
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
