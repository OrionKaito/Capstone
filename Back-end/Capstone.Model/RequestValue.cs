using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class RequestValue
    {
        [Key]
        public Guid ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public Guid RequestActionID { get; set; }
        [ForeignKey("RequestActionID")]
        public virtual RequestAction RequestAction { get; set; }
    }

}
