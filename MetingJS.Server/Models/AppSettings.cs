using Microsoft.Extensions.Configuration;

namespace MetingJS.Server.Models
{
	public class AppSettings
	{
		public static AppSettings Config { get; set; }

		public AppSettings(IConfiguration configuration)
		{
			configuration.Bind(this);
		}

		public string AllowedHosts { get; set; }
		public string[] WithOrigins { get; set; }
		public string Url { get; set; }
		public Replace Replace { get; set; }

	}

	public class Replace
	{
		public string[][] Url { get; set; }
		public string[][] Pic { get; set; }
	}
}
