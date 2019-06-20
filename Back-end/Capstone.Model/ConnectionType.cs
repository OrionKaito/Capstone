using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Model
{
    public class ConnectionType
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public StatusEnum Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}
