using Meowv.Blog.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Repositories;

namespace Meowv.Blog.Repositories
{
    public interface IFriendLinkRepository : IRepository<FriendLink, int>
    {

    }
}
