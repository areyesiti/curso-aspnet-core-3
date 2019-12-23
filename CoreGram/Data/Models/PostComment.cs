using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Models
{
    public class PostComment
    {     
        public int PostId { get; set; }
   
        public int CommentId { get; set; }        

        public Post Post { get; set; }

        public Comment Comment { get; set; }
    }
}
