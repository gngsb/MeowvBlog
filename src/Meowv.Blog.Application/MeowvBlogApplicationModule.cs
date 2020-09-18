using Meowv.Blog.Application;
using Meowv.Blog.Application.Caching;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.TenantManagement;

namespace Meowv.Blog
{
    [DependsOn(
        typeof(AbpIdentityApplicationModule),
        typeof(MeowvBlogApplicationCachingModule),
        typeof(AbpAutoMapperModule)
        )]
    public class MeowvBlogApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options => 
            {
                options.AddMaps<MeowvBlogApplicationModule>(validate: true);
                options.AddProfile<MeowvBlogAutoMapperProfile>(validate: true);
            });
        }
    }
}
