using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Model
{
    public class Role
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}
