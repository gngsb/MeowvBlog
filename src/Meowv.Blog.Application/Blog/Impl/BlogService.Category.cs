using Meowv.Blog.Application.Contracts.Blog;
using Meowv.Blog.Domain.Shared;
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
        /// 查询分类列表
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<IEnumerable<QueryCategoryDto>>> QueryCategoriesAsync()
        {
            return await _blogCacheService.QueryCategoriesAsync(async () =>
            {
                var result = new ServiceResult<IEnumerable<QueryCategoryDto>>();

                var list = from category in await _categoryRepository.GetListAsync()
                           join posts in await _postRepository.GetListAsync()
                           on category.Id equals posts.CategoryId
                           group category by new
                           {
                               category.CategoryName,
                               category.DisplayName
                           } into g
                           select new QueryCategoryDto
                           {
                               CategoryName = g.Key.CategoryName,
                               DisplayName = g.Key.DisplayName,
                               Count = g.Count()
                           };
                result.IsSuccess(list);
                return result;
            });
        }


        public async Task<ServiceResult<string>> GetCategoryAsync(string name) 
        {
            return await _blogCacheService.GetCategoryAsync(name, async () => 
            {
                var result = new ServiceResult<string>();
                var category = await _categoryRepository.FindAsync(x => x.DisplayName.Equals(name));
                if (category == null) 
                {
                    result.IsFailed(ResponseText.WHAT_NOT_EXIST.FormatWith("分类", name));
                    return result;
                }
                result.IsSuccess(category.CategoryName);
                return result;
            });
        }
    }
}
