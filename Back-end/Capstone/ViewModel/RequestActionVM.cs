using System;

namespace Capstone.ViewModel
{

    public class RequestActionVM
    {
        public Guid ID { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid RequestID { get; set; }
        public string ActorID { get; set; }
        public Guid NextStepID { get; set; }
    }

    public class RequestActionCM
    {
        public string Status { get; set; }
        public Guid RequestID { get; set; }
        public string ActorID { get; set; }
        public Guid NextStepID { get; set; }
    }

    public class RequestActionUM
    {
        public Guid ID { get; set; }
        public Guid RequestID { get; set; }
        public string ActorID { get; set; }
        public Guid NextStepID { get; set; }
    }
}
