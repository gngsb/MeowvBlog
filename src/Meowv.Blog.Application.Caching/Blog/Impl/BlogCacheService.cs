using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Meowv.Blog.Application.Caching.Blog.Impl
{
    public partial class BlogCacheService : CachingServiceBase, IBlogCacheService
    {
        public Task RemoveAsync(string key, int cursor = 0)
        {
            throw new NotImplementedException();
        }
    }
}
