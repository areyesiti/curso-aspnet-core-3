using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Models
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }                
        [MaxLength(150)]
        public string FullName { get; set; }
        public string Image { get; set; }
        public virtual User User { get; set; }
    }
}
