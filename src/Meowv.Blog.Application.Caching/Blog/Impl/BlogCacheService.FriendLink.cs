﻿using Meowv.Blog.Application.Contracts.Blog;
using Meowv.Blog.ToolKits.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Meowv.Blog.Domain.Shared.MeowvBlogDbConsts;

namespace Meowv.Blog.Application.Caching.Blog.Impl
{
    public partial class BlogCacheService
    {
        private const string KEY_QueryFriendLinks = "Blog:FriendLink:QueryFriendLinks";

        /// <summary>
        /// 查询友链列表
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<FriendLinkDto>>> QueryFriendLinksAsync(Func<Task<ServiceResult<IEnumerable<FriendLinkDto>>>> factory) 
        {
            return await Cache.GetOrAddAsync(KEY_QueryFriendLinks, factory, CacheStrategy.EIGHT_HOURS);
        }
    }
}
