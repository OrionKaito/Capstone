﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class WorkFlowTemplate
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public DateTime CreateDate { get; set; }
        public string Icon { get; set; }

        public string OwnerID { get; set; }
        [ForeignKey("OwnerID")]
        public virtual User Owner { get; set; }

        public Guid PermissionToUseID { get; set; }
        [ForeignKey("PermissionToUseID")]
        public virtual Permission PermissionToUse { get; set; }

        public bool IsViewDetail { get; set; }
        public bool IsCheckConnection { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
    }
}
