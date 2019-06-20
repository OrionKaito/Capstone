using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Model
{
    public class Role
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
