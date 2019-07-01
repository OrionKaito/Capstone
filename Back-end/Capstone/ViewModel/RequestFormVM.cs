using System;
using System.Collections.Generic;

namespace Capstone.ViewModel
{
    public class RequestFormVM
    {
        public Guid NextStepID { get; set; }

        public Guid ConnectionID { get; set; }

        public string ConnectionTypeName { get; set; }
    }

    public class FormVM
    {
        public IEnumerable<RequestFormVM> Connections { get; set; }

        public ActionTypeVM ActionType { get; set; }
    }
}
