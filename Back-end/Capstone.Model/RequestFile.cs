﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class RequestFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreateDate { get; set; }

        public Guid RequestActionID { get; set; }
        [ForeignKey("RequestActionID")]
        public RequestAction RequestAction { get; set; }

        public bool IsDeleted { get; set; }
    }
}
