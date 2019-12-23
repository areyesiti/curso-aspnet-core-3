using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Models
{
    public class User
    {        
        public int Id { get; set; }

        [MaxLength(20)]
        [Column("UserName")]
        public string Login { get; set; }

        public string Password { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public IEnumerable<Follower> UsersFollowers { get; set; }

        public IEnumerable<Follower> UsersFollowings { get; set; }

        public UserProfile Profile { get; set; }

        public IEnumerable<Post> Posts { get; set; }

        public IEnumerable<Like> Likes { get; set; }

        public IEnumerable<Comment> Comments { get; set; }


    }
}
