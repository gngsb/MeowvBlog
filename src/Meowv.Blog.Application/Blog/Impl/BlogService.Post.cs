﻿using Meowv.Blog.Application.Contracts;
using Meowv.Blog.Application.Contracts.Blog;
using Meowv.Blog.Domain.Shared;
using Meowv.Blog.Models;
using Meowv.Blog.ToolKits.Base;
using Meowv.Blog.ToolKits.Extensions;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Meowv.Blog.Application.Blog.Impl
{
    public partial class BlogService
    {
        /// <summary>
        /// 分页查询文章列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PagedList<QueryPostDto>>> QueryPostsAsync(PagingInput input)
        {
            return await _blogCacheService.QueryPostsAsync(input, async () =>
            {
                var result = new ServiceResult<PagedList<QueryPostDto>>();

                #region Posts
                var count = await _postRepository.GetCountAsync();
                var list = _postRepository.OrderByDescending(x => x.CreationTime).PageByIndex(input.Page, input.Limit).Select(x => new PostBriefDto
                {
                    Title = x.Title,
                    Url = x.Url,
                    Year = x.CreationTime.Year,
                    CreationTime = x.CreationTime.TryToDateTime()
                }).GroupBy(x => x.Year).Select(x => new QueryPostDto
                {
                    Year = x.Key,
                    Posts = x.ToList()
                }).ToList();
                #endregion

                #region HotNews
                //var count = await _hotNewsRepository.GetCountAsync();
                //var list = _hotNewsRepository.OrderByDescending(x => x.CreateTime).PageByIndex(input.Page, input.Limit).Select(x => new PostBriefDto
                //{
                //    Title = x.Title,
                //    Url = x.Url,
                //    Year = x.CreateTime.Year,
                //    CreationTime = x.CreateTime.TryToDateTime()
                //}).GroupBy(x => x.Year).Select(x => new QueryPostDto
                //{
                //    Year = x.Key,
                //    Posts = x.ToList()
                //}).ToList();
                #endregion

                result.IsSuccess(new PagedList<QueryPostDto>(count.TryToInt(), list));

                return result;
            });
        }

        /// <summary>
        /// 根据URL获取文章详情
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ServiceResult<PostDetailDto>> GetPostDetailAsync(string url)
        {
            return await _blogCacheService.GetPostDetailAsync(url, async () =>
            {
                var result = new ServiceResult<PostDetailDto>();
                var post = await _postRepository.FindAsync(x => x.Url.Equals(url));
                if (post == null)
                {
                    result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("URL", url));
                    return result;
                }

                var category = await _categoryRepository.GetAsync(post.CategoryId);

                var tags = from post_tags in await _postTagRepository.GetListAsync()
                           join tag in await _tagRepository.GetListAsync()
                           on post_tags.TagId equals tag.Id
                           where post_tags.PostId.Equals(post.Id)
                           select new TagDto
                           {
                               TagName = tag.TagName,
                               DisplayName = tag.DisplayName
                           };
                //获取上一条信息
                var previous = _postRepository.Where(x => x.CreationTime < post.CreationTime).OrderByDescending(x => x.CreationTime).Take(1).FirstOrDefault();
                //获取下一条信息
                var next = _postRepository.Where(x => x.CreationTime > post.CreationTime).OrderBy(x => x.CreationTime).Take(1).FirstOrDefault();

                var postDetail = new PostDetailDto 
                {
                    Title = post.Title,
                    Author = post.Author,
                    Url = post.Url,
                    Html = post.Html,
                    Markdown = post.Markdown,
                    CreationTime = post.CreationTime.TryToDateTime(),
                    Category = new CategoryDto 
                    {
                        CategoryName = category.CategoryName,
                        DisplayName = category.DisplayName
                    },
                    Tags = tags,
                    Previous = previous == null ? null : new PostForPagedDto
                    {
                        Title = previous.Title,
                        Url = previous.Url
                    },
                    Next = next == null ? null : new PostForPagedDto
                    {
                        Title = next.Title,
                        Url = next.Url
                    }
                };

                result.IsSuccess(postDetail);
                return result;

            });

        }
    }
}
