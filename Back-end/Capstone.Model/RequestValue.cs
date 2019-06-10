﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public  class RequestValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public string Data { get; set; }

        public Guid RequestActionID { get; set; }
        [ForeignKey("RequestActionID")]
        public RequestAction RequestAction { get; set; }
    }

}
