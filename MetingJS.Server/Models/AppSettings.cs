using Meting4Net.Core;

namespace MetingJS.Server.Models;

public class AppSettings
{
    public AppSettings()
    {
    }

    public AppSettings(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public KestrelSettings KestrelSettings { get; set; } = new();

    public List<string> WithOrigins { get; set; }
    public ServerProvider DefaultServerProvider { get; set; } = ServerProvider.Tencent;
    public string Url { get; set; }
    public Replace Replace { get; set; } = new();
    public Cache Cache { get; set; }
}

public class Cache
{
    public string Directory { get; set; } = "DataBase";
    public string CacheDataBase { get; set; } = "Caching";
    public int Base { get; set; } = 120;
    public int Url { get; set; } = 120;
    public int Pic { get; set; } = 120;
    public int Lrc { get; set; } = 43200;
}

public class Replace
{
    public List<List<string>>? Url { get; set; }
    public List<List<string>>? Pic { get; set; }
}

public class KestrelSettings
{
    /// <summary>
    ///     服务运行端口
    /// </summary>
    public Dictionary<string, List<int>> Listens { get; set; } = new();

    /// <summary>
    ///     UnixSocketPath
    /// </summary>
    public List<string>? UnixSocketPath { get; set; }
}