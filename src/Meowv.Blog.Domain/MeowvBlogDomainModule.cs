﻿using Meowv.Blog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Meowv.Blog
{
    [DependsOn(typeof(AbpIdentityDomainModule),
        typeof(MeowvBlogDomainSharedModule))]
    public class MeowvBlogDomainModule : AbpModule
    {

    }
}
