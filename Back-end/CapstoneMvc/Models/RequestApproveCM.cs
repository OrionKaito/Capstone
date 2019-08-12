using System;

namespace CapstoneMvc.Models
{
    public class RequestApproveCM
    {
        //Request Action
        public Guid RequestID { get; set; }

        public Guid RequestActionID { get; set; }

        public Guid NextStepID { get; set; }
    }
}
