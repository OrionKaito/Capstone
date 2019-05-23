using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Capstone.Model
{
    public class UserGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public Guid GroupID { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
        public bool IsDelete { get; set; }
    }
}
