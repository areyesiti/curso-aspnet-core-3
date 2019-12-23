using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [MaxLength(150)]
        public string Remark { get; set; }

        public DateTime Date { get; set; }

        public User User { get; set; }

        public IEnumerable<PostComment> PostsComments { get; set; }
    }
}
