using Aliyun.Serverless.Core.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MetingJS.Server.Fc
{
    public class FcRemoteEntrypoint : FcHttpEntrypoint
    {
        protected override void Init(IWebHostBuilder webBuilder)
        {
            webBuilder.ConfigureAppConfiguration((context, builder) =>
            {
                var env = context.HostingEnvironment;
                builder.AddJsonFile("appsettings.json", true, true)
                       .AddYamlFile("appsettings.yml", true, true)
                       .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                       .AddJsonFile($"appsettings.{env.EnvironmentName}.yml", true);
            }).UseStartup<Startup>();
        }
    }
}
