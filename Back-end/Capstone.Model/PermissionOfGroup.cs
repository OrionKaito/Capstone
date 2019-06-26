using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class PermissionOfGroup
    {
        [Key]
        public Guid ID { get; set; }

        public Guid PermissionID { get; set; }
        [ForeignKey("PermissionID")]
        public virtual Permission Permission { get; set; }

        public Guid GroupID { get; set; }
        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }

        public bool IsDeleted { get; set; }
    }
}
