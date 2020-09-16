using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Meowv.Blog.Swagger.Filters
{
    /// <summary>
    /// 对应Controller的API文档描述信息
    /// </summary>
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var tags = new List<OpenApiTag>
            {
                new OpenApiTag {
                    Name = "Blog",
                    Description = "个人博客相关接口",
                    ExternalDocs = new OpenApiExternalDocs { Description = "包含：文章/标签/分类/友链" }
                },
                new OpenApiTag {
                    Name = "HelloWorld",
                    Description = "通用公共接口",
                    ExternalDocs = new OpenApiExternalDocs { Description = "这里是一些通用的公共接口" }
                },
                new OpenApiTag{
                    Name="Auth",
                    Description = "JWT模式认证授权",
                    ExternalDocs = new OpenApiExternalDocs { Description = "JSON Web Token" }
                }
            };

            #region 实现添加自定义描述时过滤不属于同一个分组的API
            //从context.ApiDescriptions获取到当前显示的是哪一个分组下的API
            var groupName = context.ApiDescriptions.FirstOrDefault().GroupName;
            //然后使用GetType().GetField(string name, BindingFlags bindingAttr)获取到_source，当前项目的所有API，里面同时也包含了ABP默认生成的一些接口
            var apis = context.ApiDescriptions.GetType().GetField("_source", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(context.ApiDescriptions) as IEnumerable<ApiDescription>;
            //再将API中不属于当前分组的API筛选掉，用Select查询出所有的Controller名称进行去重。
            var controllers = apis.Where(x => x.GroupName != groupName).Select(x => ((ControllerActionDescriptor)x.ActionDescriptor).ControllerName).Distinct();
            //因为OpenApiTag中的Name名称与Controller的Name是一致的，所以最后将包含controllers名称的tag查询出来取反，即可满足需求
            swaggerDoc.Tags = tags.Where(x => !controllers.Contains(x.Name)).OrderBy(x => x.Name).ToList();
            #endregion

            //swaggerDoc.Tags = tags.OrderBy(x => x.Name).ToList();

            //throw new NotImplementedException();
        }
    }
}
