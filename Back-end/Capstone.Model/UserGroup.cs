using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class UserGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        public Guid GroupID { get; set; }
        [ForeignKey("GroupID")]
        public virtual Group Group { get; set; }

        public bool IsDelete { get; set; }
    }
}
