using Meowv.Blog.Application.Contracts;
using Meowv.Blog.Application.Contracts.Blog;
using Meowv.Blog.ToolKits.Base;
using Meowv.Blog.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var count1 = await _hotNewsRepository.GetCountAsync();
                var list1 = _hotNewsRepository.OrderByDescending(x => x.CreateTime).PageByIndex(input.Page, input.Limit).Select(x => new PostBriefDto
                {
                    Title = x.Title,
                    Url = x.Url,
                    Year = x.CreateTime.Year,
                    CreationTime = x.CreateTime.TryToDateTime()
                }).GroupBy(x => x.Year).Select(x => new QueryPostDto
                {
                    Year = x.Key,
                    Posts = x.ToList()
                }).ToList();
                #endregion

                result.IsSuccess(new PagedList<QueryPostDto>(count1.TryToInt(), list1));

                return result;
            });
        }
    }
}
