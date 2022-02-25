using System.Text.Json;
using Meting4Net.Core;
using MetingJS.Server.Models;
using MMeting = Meting4Net.Core.Meting;
using static MetingJS.Server.Models.QueryType;

namespace MetingJS.Server.Utils;

public class Meting
{
    private readonly CacheLiteDb _caching;
    private readonly AppSettings _config;
    private readonly MMeting _meting;

    public Meting(AppSettings appSettings, CacheLiteDb cache)
    {
        _config = appSettings;
        _meting = new MMeting(appSettings.DefaultServerProvider);
        _caching = cache;
    }

    public Meting SetServer(ServerProvider server)
    {
        _meting.Server = server;
        return this;
    }

    public async Task<string?> GetLrcAsync(string id)
    {
        return await _caching.GetAsync($"{nameof(GetLrcAsync)}.{id}", async () =>
            {
                return await Task.Run(() =>
                {
                    var lrc = _meting.LyricObj(id);
                    return lrc.lyric;
                });
            },
            TimeSpan.FromMinutes(_config.Cache.Lrc));
    }

    public async Task<string?> GetPicAsync(string id)
    {
        return await _caching.GetAsync($"{nameof(GetPicAsync)}.{id}", async () =>
            {
                return await Task.Run(() =>
                {
                    var pic = _meting.PicObj(id, 90);
                    var picUrl = pic.url;
                    if (!string.IsNullOrEmpty(picUrl))
                        picUrl = Replace(picUrl, _config.Replace.Pic);
                    return picUrl;
                });
            },
            TimeSpan.FromMinutes(_config.Cache.Pic));
    }

    public async Task<string?> GetUrlAsync(string id)
    {
        return await _caching.GetAsync($"{nameof(GetUrlAsync)}.{id}", async () =>
            {
                return await Task.Run(() =>
                {
                    var url = _meting.UrlObj(id);
                    var urlUrl = url.url;
                    if (!string.IsNullOrEmpty(urlUrl))
                        urlUrl = Replace(urlUrl, _config.Replace.Url);
                    return urlUrl;
                });
            },
            TimeSpan.FromMinutes(_config.Cache.Url));
    }

    public async Task<string?> SearchAsync(QueryModel query, string baseUrl)
    {
        var list = await _caching.GetAsync($"{nameof(SearchAsync)}.{query.Id}", async () =>
            {
                return await Task.Run(() =>
                {
                    MusicList[] musicList = { };
                    switch (query.Type)
                    {
                        case Album:
                            musicList = _meting.AlbumObj(query.Id).Select(s => new MusicList(s, query.Server, baseUrl))
                                .ToArray();
                            break;
                        case Artist:
                            musicList = _meting.ArtistObj(query.Id).Select(s => new MusicList(s, query.Server, baseUrl))
                                .ToArray();
                            break;
                        case PlayList:
                            musicList = _meting.PlaylistObj(query.Id)
                                .Select(s => new MusicList(s, query.Server, baseUrl)).ToArray();
                            break;
                        case Search:
                            musicList = _meting.SearchObj(query.Id).Select(s => new MusicList(s, query.Server, baseUrl))
                                .ToArray();
                            break;
                        case Song:
                            musicList = new MusicList[] { new(_meting.SongObj(query.Id), query.Server, baseUrl) };
                            break;
                    }

                    return musicList;
                });
            },
            TimeSpan.FromMinutes(_config.Cache.Base));

        return JsonSerializer.Serialize(list);
    }

    private string Replace(string url, List<List<string>>? p)
    {
        if (p != null)
            foreach (var v in p)
                url = url.Replace(v[0], v[1]);
        return url;
    }
}