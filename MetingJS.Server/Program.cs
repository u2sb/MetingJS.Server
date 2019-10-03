using Microsoft.AspNetCore.Hosting;
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
	}
}
