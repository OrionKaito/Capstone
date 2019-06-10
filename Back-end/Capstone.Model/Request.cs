using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone.Model
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public DateTime CreateDate { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }

        public bool IsDeleted { get; set; }
    }
}
