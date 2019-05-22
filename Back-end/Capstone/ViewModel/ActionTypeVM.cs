using System;

namespace Capstone.ViewModel
{
    public class ActionTypeVM
    {
        public Guid ActionTypeID { get; set; }
        public string Name { get; set; }
    }

    public class ActionTypeCM
    {
        public string Name { get; set; }
    }

    public class ActionTypeUM
    {
        public Guid ActionTypeID { get; set; }
        public string Name { get; set; }
    }
}
