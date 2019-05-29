using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class PermissionOfRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public Guid PermissionID { get; set; }
        [ForeignKey("PermissionID")]
        public virtual Permission Permission { get; set; }

        public Guid RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }

        public bool IsDelete { get; set; }
    }
}
