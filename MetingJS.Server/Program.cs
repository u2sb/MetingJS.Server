#if NETCORE31
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
#elif NETCORE21
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
#endif

#if LINUX
using System.IO;
#endif

namespace MetingJS.Server
{
	public class Program
	{
#if NETCORE21
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                       .UseStartup<Startup>();
#elif NETCORE31

		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>().ConfigureKestrel((context, options) =>
					{
#if LINUX
                        if (File.Exists("/tmp/metingJS.Server.sock")) File.Delete("/tmp/metingJS.Server.sock");
                        options.ListenUnixSocket("/tmp/metingJS.Server.sock");
#endif
					});
				});
#endif
	}

}

