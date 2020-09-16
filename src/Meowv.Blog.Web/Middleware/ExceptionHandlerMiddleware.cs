using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Meowv.Blog.ToolKits.Base;
using Meowv.Blog.ToolKits.Extensions;

namespace Meowv.Blog.HttpApi.Hosting.Middleware
{
    /// <summary>
    /// 异常处理中间件
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next) 
        {
            this.next = next;
        }

        /// <summary>
        /// Invoke(援引，引用)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context) 
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await ExceptionHandlerAsync(context, ex.Message);
            }
            finally 
            {
                var statusCode = context.Response.StatusCode;
                if (statusCode != StatusCodes.Status200OK) 
                {
                    Enum.TryParse(typeof(HttpStatusCode), statusCode.ToString(), out object message);
                    await ExceptionHandlerAsync(context, message.ToString());
                }
            }
        }

        private async Task ExceptionHandlerAsync(HttpContext context, string message) 
        {
            context.Response.ContentType = "application/json;charset=utf-8";

            var result = new ServiceResult();
            result.IsFailed(message);

            await context.Response.WriteAsync(result.ToJson());
        }
    }
}
