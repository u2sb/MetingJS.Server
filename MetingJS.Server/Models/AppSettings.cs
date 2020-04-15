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

        public string[] WithOrigins { get; } = new string[0];
        public ServerProvider DefaultServerProvider { get; } = ServerProvider.Tencent;
        public string Url { get; } = "";
        public Replace Replace { get; } = new Replace();
    }

    public class Replace
    {
        public string[][] Url { get; set; }
        public string[][] Pic { get; set; }
    }
}
