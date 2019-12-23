using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreGram.Data.Dto;
using CoreGram.Data.Models;
using Profile = AutoMapper.Profile;

namespace CoreGram.Helpers
{
    /// <summary>
    /// Clase de perfil de mapeos para Automapper
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>();            
            CreateMap<User, UserDto>();                        
            CreateMap<UserInfoDto, User>();
            CreateMap<User, UserInfoDto>();

            CreateMap<UserProfileDto, UserProfile>();
            CreateMap<UserProfile, UserProfileDto>();

            CreateMap<FollowerDto, Follower>();
            CreateMap<Follower, FollowerDto>();
            CreateMap<Follower, FollowerInfoDto>();

            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();

            CreateMap<PostDto, Post>();
            CreateMap<Post, PostDto>();
            CreateMap<PostInfoDto, Post>();                        
            CreateMap<LikeDto, Like>();
            CreateMap<Like, LikeDto>();
        }
    }
}
