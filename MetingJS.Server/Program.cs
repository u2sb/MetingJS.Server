#if NETCORE31
using System.Net;
using MetingJS.Server.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
#if LINUX
using System.IO;
#endif

namespace MetingJS.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var appSettings = new AppSettings();

            return Host.CreateDefaultBuilder(args)
                       .ConfigureAppConfiguration((context, builder) =>
                        {
                            var env = context.HostingEnvironment;
                            builder.AddJsonFile("appsettings.json", true, true)
                                   .AddYamlFile("appsettings.yml", true, true)
                                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                                   .AddJsonFile($"appsettings.{env.EnvironmentName}.yml", true);
                            appSettings = new AppSettings(builder.Build());
                        })
                       .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.ConfigureKestrel(options =>
                            {
                                var ks = appSettings.KestrelSettings;
#if LINUX
                                if (ks.UnixSocketPath.Length > 0)
                                    foreach (var path in ks.UnixSocketPath)
                                    {
                                        if (File.Exists(path)) File.Delete(path);
                                        options.ListenUnixSocket(path);
                                    }
#endif
                                if (ks.Port.Length > 0)
                                    foreach (var port in ks.Port)
                                        options.Listen(IPAddress.Loopback, port);
                            }).UseStartup<Startup>();
                        });
        }
    }
}
#endif

#if NETCORE21
using System;

namespace MetingJS.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("请不要这样启动");
        }
    }
}
#endif
