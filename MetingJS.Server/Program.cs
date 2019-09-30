using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
#if DEBUG || WIN
						options.ListenAnyIP(5001);
#elif LINUX
                        if (File.Exists("/tmp/metingJS.Server.sock")) File.Delete("/tmp/metingJS.Server.sock");
                        options.ListenUnixSocket("/tmp/metingJS.Server.sock");
#endif
					});
				});
	}
}
