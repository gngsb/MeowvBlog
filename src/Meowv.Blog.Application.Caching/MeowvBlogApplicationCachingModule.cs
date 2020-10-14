using Meowv.Blog.Configurations;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Meowv.Blog.Application.Caching
{
    [DependsOn(
        typeof(AbpCachingModule),
        typeof(MeowvBlogDomainModule)
    )]
    public class MeowvBlogApplicationCachingModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //base.ConfigureServices(context);
            context.Services.AddStackExchangeRedisCache(options => 
            {
                // Redis 的连接字符串
                options.Configuration = AppSettings.Caching.RedisConnectionString;
                //Redis 实例名称
                //options.InstanceName
                //Redis 的配置属性，如果配置了这个字，将优先于 Configuration 中的配置，同时它支持更多的选项
                //options.ConfigurationOptions
            });

            var csredis = new CSRedis.CSRedisClient(AppSettings.Caching.RedisConnectionString);
            RedisHelper.Initialization(csredis);
            context.Services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));

        }
    }
}
