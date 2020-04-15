using Meting4Net.Core;
using Microsoft.Extensions.Configuration;

namespace MetingJS.Server.Models
{
    public class AppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

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
}
