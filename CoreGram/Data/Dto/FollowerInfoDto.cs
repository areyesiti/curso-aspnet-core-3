using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Dto
{
    public class FollowerInfoDto
    {
        public int UserId { get; set; }
        public int FollowerId { get; set; }
        public DateTime Date { get; set; }

        public UserInfoDto UserFollower { get; set; }
        public UserInfoDto UserFollowing { get; set; }
    }
}
