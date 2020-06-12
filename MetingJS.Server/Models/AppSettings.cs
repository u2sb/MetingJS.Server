using Meting4Net.Core;
using Microsoft.Extensions.Configuration;

namespace MetingJS.Server.Models
{
    public class AppSettings
    {
        public AppSettings() { }

        public AppSettings(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

        public KestrelSettings KestrelSettings { get; set; } = new KestrelSettings();

        public string[] WithOrigins { get; set; }
        public ServerProvider DefaultServerProvider { get; set; } = ServerProvider.Tencent;
        public string Url { get; set; }
        public Replace Replace { get; set; } = new Replace();
    }

    public class Replace
    {
        public string[][] Url { get; set; }
        public string[][] Pic { get; set; }
    }

    public class KestrelSettings
    {
        /// <summary>
        ///     服务运行端口
        /// </summary>
        public int[] Port { get; set; }

        /// <summary>
        ///     UnixSocketPath
        /// </summary>
        public string[] UnixSocketPath { get; set; }
    }
}
