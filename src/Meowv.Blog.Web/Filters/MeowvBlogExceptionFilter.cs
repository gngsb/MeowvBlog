using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meowv.Blog.HttpApi.Hosting.Filters
{
    public class MeowvBlogExceptionFilter : IExceptionFilter
    {

        private readonly ILog _log;

        public MeowvBlogExceptionFilter() 
        {
            _log = LogManager.GetLogger(typeof(MeowvBlogExceptionFilter));
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="continstall-packageext"></param>
        public void OnException(ExceptionContext context)
        {
            //日志记录
            _log.Error($"{context.HttpContext.Request.Path}|{context.Exception.Message}", context.Exception);
        }
    }
}
