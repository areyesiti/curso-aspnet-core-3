using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
