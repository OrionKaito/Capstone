﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class RequestAction
    {
        [Key]
        public Guid ID { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime CreateDate { get; set; }

        public Guid RequestID { get; set; }
        [ForeignKey("RequestID")]
        public virtual Request Request { get; set; }

        public string ActorID { get; set; }
        [ForeignKey("ActorID")]
        public virtual User User { get; set; }

        public Guid? NextStepID { get; set; }
        [ForeignKey("NextStepID")]
        public virtual WorkFlowTemplateAction WorkFlowTemplateAction { get; set; }

        public bool IsDeleted { get; set; }
    }
}
