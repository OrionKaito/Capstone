using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Capstone.Model
{
    public class RequestFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public String name { get; set; }
        public String path { get; set; }
        public DateTime timeStamp { get; set; }
        public bool isEnable { get; set; }
    }
}
