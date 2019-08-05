using Capstone.Model;
using System;

namespace CapstoneMvc.Models
{
    public class RequestApproveCM
    {
        //Request Action
        public Guid RequestID { get; set; }

        public Guid RequestActionID { get; set; }

        public StatusEnum Status { get; set; }

        public Guid NextStepID { get; set; }

        public string ActorEmail { get; set; }

        //Request Value
        //public IEnumerable<ActionValueVM> ActionValues { get; set; }
    }

    public class ActionValueVM
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
