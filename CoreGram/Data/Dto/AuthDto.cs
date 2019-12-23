using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Dto
{
    public class AuthDto
    {
        public string Token { get; set; }
        public UserInfoDto UserInfo  { get; set; }
    }
}
