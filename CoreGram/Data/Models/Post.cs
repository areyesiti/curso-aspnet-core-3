using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Models
{
    public class Post
    {        
        public int Id { get; set; }

        public int UserId { get; set; }

        [Column("Photo", TypeName = "nvarchar(MAX)")]
        public string Picture { get; set; }

        public DateTime Date { get; set; }

        public User User { get; set; }

        public IEnumerable<Like> Likes { get; set; }

        public IEnumerable<PostComment> PostsComments { get; set; }
    }
}
