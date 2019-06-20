using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Model
{
    public class ActionType
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public bool IsDeleted { get; set; }
    }
}
