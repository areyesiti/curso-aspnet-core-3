using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Models
{    
    public class Follower
    {
        public int UserId { get; set; }

        public int FollowerId { get; set; }  
        
        public DateTime Date { get; set; }        

        public User UserFollower { get; set; }
        public User UserFollowing { get; set; }
    }
}
