using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class WorkFlowTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WorkFlowTemplateID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Actor { get; set; }

        public string OwnerID { get; set; }
        [ForeignKey("UserID")]
        public virtual User Owner { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
    }
}
