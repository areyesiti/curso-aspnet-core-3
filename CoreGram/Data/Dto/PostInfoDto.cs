using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGram.Data.Models;

namespace CoreGram.Data.Dto
{
    public class PostInfoDto
    {
        public int Id { get; set; }        
        public UserInfoDto User { get; set; }
        public string Picture { get; set; }
        public int Likes { get; set; }
        public int TotalComments { get; set; }
        public List<CommentInfoDto> Comments { get; set; }
    }
}
