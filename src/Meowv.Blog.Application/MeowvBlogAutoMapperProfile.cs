using AutoMapper;
using Meowv.Blog.Application.Contracts.Blog;
using Meowv.Blog.Application.Contracts.Blog.Params;
using Meowv.Blog.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meowv.Blog.Application
{
    public class MeowvBlogAutoMapperProfile:Profile
    {
        public MeowvBlogAutoMapperProfile() 
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<FriendLink, FriendLinkDto>();
            CreateMap<EditPostInput, Post>().ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Post, PostForAdminDto>().ForMember(x => x.Tags, opt => opt.Ignore());
        }
    }
}
