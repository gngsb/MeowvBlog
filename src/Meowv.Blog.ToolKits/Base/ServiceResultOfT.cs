using Meowv.Blog.ToolKits.Base.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meowv.Blog.ToolKits.Base
{
    /// <summary>
    /// class A ：where T : class 泛型约束，此约束表示类型参数必须是引用类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T>:ServiceResult where T : class 
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        public void IsSuccess(T result = null, string message = "") 
        {
            Message = message;
            Code = ServiceResultCode.Succeed;
            Result = result;
        }
    }
}
