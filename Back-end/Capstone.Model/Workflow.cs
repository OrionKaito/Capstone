using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class WorkFlow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WorkFlowID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string OwnerID { get; set; }
        [ForeignKey("UserID")]
        public virtual User Owner { get; set; }

        public bool IsDeleted { get; set; }
    }
}
