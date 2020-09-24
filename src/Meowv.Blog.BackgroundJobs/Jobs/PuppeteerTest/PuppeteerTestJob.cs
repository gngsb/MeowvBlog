using Meowv.Blog.ToolKits.Extensions;
using Meowv.Blog.ToolKits.Helper;
using MimeKit;
using MimeKit.Utils;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Meowv.Blog.BackgroundJobs.Jobs.PuppeteerTest
{
    public class PuppeteerTestJob : IBackgroundJob
    {
        public async Task ExecuteAsync()
        {
            string path1 = Environment.CurrentDirectory;//取得或设置当前工作目录的完整限定路径
            //string path2 = Directory.GetCurrentDirectory();//获取应用程序的当前工作目录
            var path = Path.Combine(path1, "meowv.png");
            //第一次检测到没有浏览器文件会默认帮我们下载 chromium 浏览器。
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            //配置浏览器启动的方式。
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions 
            {
                //以无头模式运行浏览器
                Headless = true,
                //针对Linux环境下，如果是运行在 root 权限下，在启动 Puppeteer 时要添加 "--no-sandbox" 参数，否则 Chromium 会启动失败。
                Args = new string[] { "--no-sandbox" }
            });

            using var page = await browser.NewPageAsync();

            //设置网页预览大小
            await page.SetViewportAsync(new ViewPortOptions 
            {
                Width = 1920,
                Height = 1080
            });

            var url = "http://ppt.cd0dhd10.com.cn/";
            await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
            var content = await page.GetContentAsync();

            //生成PDF和保存图片
            await page.PdfAsync("meowv.pdf", new PdfOptions { });
            await page.ScreenshotAsync("meowv.png", new ScreenshotOptions 
            {
                FullPage = true,
                Type = ScreenshotType.Png
            });

            //发送带图片的Email
            var builder = new BodyBuilder();
            var image = builder.LinkedResources.Add(path);
            image.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = "当前时间：{0}.<img src=\"cid:{1}\" />".FormatWith(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), image.ContentId);

            var message = new MimeMessage 
            {
                Subject = "【定时任务】定时发送图片",
                Body = builder.ToMessageBody()
            };
            await EmailHelper.SendAsync(message);

        }
    }
}
