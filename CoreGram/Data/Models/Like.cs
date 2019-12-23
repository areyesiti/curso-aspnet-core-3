using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Models
{    
    public class Like
    {       
        public int PostId { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }        

        public User User { get; set; }

        public Post Post { get; set; }
    }
}
