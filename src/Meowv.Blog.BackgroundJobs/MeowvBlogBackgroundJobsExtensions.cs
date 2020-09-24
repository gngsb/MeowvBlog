using Hangfire;
using log4net.Core;
using Meowv.Blog.BackgroundJobs.Jobs.Hangfire;
using NUglify.JavaScript.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Meowv.Blog.BackgroundJobs.Jobs.Wallpaper;
using Meowv.Blog.BackgroundJobs.Jobs.HotNews;
using Meowv.Blog.BackgroundJobs.Jobs.PuppeteerTest;

namespace Meowv.Blog.BackgroundJobs
{
    public static class MeowvBlogBackgroundJobsExtensions
    {
        public static void UseHangfireTest(this IServiceProvider service) 
        {
            var job = service.GetService<HangfireTestJob>();
            RecurringJob.AddOrUpdate("定时任务测试", () => job.ExecuteAsync(), CronType.Minute());
        }

        public static void UseWallpaperJob(this IServiceProvider service)
        {
            var job = service.GetService<WallpaperJob>();
            RecurringJob.AddOrUpdate("壁纸数据抓取", () => job.ExecuteAsync(), CronType.Hour(1, 3));
        }

        public static void UseHotNewsJob(this IServiceProvider service) 
        {
            var job = service.GetService<HotNewsJob>();
            RecurringJob.AddOrUpdate("每日热点数据抓取", () => job.ExecuteAsync(), CronType.Hour(1, 2));
        }

        public static void UsePuppeteerTestJob(this IServiceProvider service) 
        {
            var job = service.GetService<PuppeteerTestJob>();
            BackgroundJob.Enqueue(() => job.ExecuteAsync());
        }

    }
}
