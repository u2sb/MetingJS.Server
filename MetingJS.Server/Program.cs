using System.Net;
using EasyCaching.LiteDB;
using LiteDB;
using MetingJS.Server.Models;
using MetingJS.Server.Utils;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
#if LINUX
#endif


var builder = WebApplication.CreateBuilder(args);
var appSettings = builder.Configuration.Get<AppSettings>();
if (!Directory.Exists(appSettings.Cache.Directory)) Directory.CreateDirectory(appSettings.Cache.Directory);

builder.WebHost.ConfigureKestrel(options =>
{
    var ks = appSettings.KestrelSettings;
    if (ks.Listens.Count > 0)
        foreach (var listen in ks.Listens)
            if (IPAddress.TryParse(listen.Key, out var ip))
                foreach (var port in listen.Value)
                    options.Listen(ip, port, op => { op.Protocols = HttpProtocols.Http1AndHttp2AndHttp3; });

#if LINUX
    if (ks.UnixSocketPath?.Count > 0)
        foreach (var path in ks.UnixSocketPath)
        {
            if (File.Exists(path)) File.Delete(path);
            options.ListenUnixSocket(path, op => { op.Protocols = HttpProtocols.Http1AndHttp2AndHttp3; });
        }
#endif
});

var services = builder.Services;


// 转接头，代理
services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// 跨域
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.SetIsOriginAllowedToAllowWildcardSubdomains()
        .WithOrigins(appSettings.WithOrigins.ToArray())
        .WithMethods("GET", "POST", "OPTIONS")
        .AllowAnyHeader());
});

// 缓存
services.AddEasyCaching(options =>
{
    options.UseLiteDB(config =>
    {
        config.DBConfig = new LiteDBDBOptions
        {
            ConnectionType = ConnectionType.Direct,
            FilePath = appSettings.Cache.Directory,
            FileName = appSettings.Cache.CacheDataBase
        };
    }, "LiteDb");
});

services.AddSingleton(appSettings);
services.AddSingleton<CacheLiteDb>();
services.AddScoped<Meting>();
services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseCors();

app.UseRouting();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();