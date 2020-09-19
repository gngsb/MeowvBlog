using Meowv.Blog.HotNews.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Meowv.Blog.EntityFrameworkCore.Repositories.HotNews
{
    public class HotNewsRepository : EfCoreRepository<MeowvBlogDbContext, Meowv.Blog.HotNews.HotNews, Guid>,IHotNewsRepository
    {
        public HotNewsRepository(IDbContextProvider<MeowvBlogDbContext> dbContextProvider) : base(dbContextProvider) 
        {
            
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="hotNews"></param>
        /// <returns></returns>
        public async Task BulkInsertAsync(IEnumerable<Meowv.Blog.HotNews.HotNews> hotNews)
        {
            await DbContext.Set<Meowv.Blog.HotNews.HotNews>().AddRangeAsync(hotNews);
            await DbContext.SaveChangesAsync();
        }
    }
}
