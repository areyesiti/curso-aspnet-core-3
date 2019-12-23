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
        public int Id { get; set; }

        [MaxLength(150)]
        public string FullName { get; set; }

        public string Image { get; set; }

        // Propiedad para definir la relación en base a convenciones de Data Annotations
        // public int UserId { get; set; }

        public User User { get; set; }
    }
}
