using Capstone.Model;
using System;

namespace Capstone.ViewModel
{

    public class RequestActionVM
    {
        public Guid ID { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid RequestID { get; set; }
        public string ActorID { get; set; }
        public Guid NextStepID { get; set; }
    }

    public class RequestActionCM
    {
        public Guid RequestID { get; set; }
        public Guid NextStepID { get; set; }
        public StatusEnum Status { get; set; }
    }

    public class RequestActionUM
    {
        //public Guid ID { get; set; }
        //public Guid RequestID { get; set; }
        //public string ActorID { get; set; }
        //public Guid NextStepID { get; set; }
    }
}
