using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Dto
{
    public class PostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Picture { get; set; }
        public DateTime Date { get; set; }        
    }
}
