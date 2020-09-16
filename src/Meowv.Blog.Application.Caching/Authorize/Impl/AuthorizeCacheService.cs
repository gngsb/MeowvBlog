using Meowv.Blog.ToolKits.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Meowv.Blog.Application.Caching.Authorize.Impl
{
    public class AuthorizeCacheService : IAuthorizeCacheService
    {
        public Task<ServiceResult<string>> GenerateTokenAsync(string access_token, Func<Task<ServiceResult<string>>> factory)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<string>> GetAccessTokenAsync(string code, Func<Task<ServiceResult<string>>> factory)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<string>> GetLoginAddressAsync(Func<Task<ServiceResult<string>>> factory)
        {
            throw new NotImplementedException();
        }
    }
}
