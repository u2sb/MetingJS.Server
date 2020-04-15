using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Meting4Net.Core;
using MetingJS.Server.Models;
using MMeting = Meting4Net.Core.Meting;
using static MetingJS.Server.Models.QueryType;

namespace MetingJS.Server.Utils
{
    public class Meting
    {
        private readonly AppSettings _config;
        private readonly MMeting _meting;

        public Meting(AppSettings appSettings)
        {
            _config = appSettings;
            _meting = new MMeting(appSettings.DefaultServerProvider);
        }

        public Meting SetServer(ServerProvider server)
        {
            _meting.Server = server;
            return this;
        }

        public string GetLrc(string id)
        {
            var lrc = _meting.LyricObj(id);
            return lrc.lyric;
        }

        public string GetPic(string id)
        {
            var pic = _meting.PicObj(id, 90);
            var picUrl = pic.url;
            if (!string.IsNullOrEmpty(picUrl) && _config.Replace.Pic != null)
                picUrl = Replace(picUrl, _config.Replace.Pic);
            return picUrl;
        }

        public string GetUrl(string id)
        {
            var url = _meting.UrlObj(id);
            var urlUrl = url.url;
            if (!string.IsNullOrEmpty(urlUrl) && _config.Replace.Url != null)
                urlUrl = Replace(urlUrl, _config.Replace.Url);
            return urlUrl;
        }

        public string Search(QueryModel query, string baseUrl)
        {
            var list = new List<MusicList>();
            switch (query.Type)
            {
                case Album:
                    list = _meting.AlbumObj(query.Id).Select(s => new MusicList(s, query.Server, baseUrl)).ToList();
                    break;
                case Artist:
                    list = _meting.ArtistObj(query.Id).Select(s => new MusicList(s, query.Server, baseUrl)).ToList();
                    break;
                case PlayList:
                    list = _meting.PlaylistObj(query.Id).Select(s => new MusicList(s, query.Server, baseUrl)).ToList();
                    break;
                case QueryType.Search:
                    list = _meting.SearchObj(query.Id).Select(s => new MusicList(s, query.Server, baseUrl)).ToList();
                    break;
                case Song:
                    list = new List<MusicList> {new MusicList(_meting.SongObj(query.Id), query.Server, baseUrl)};
                    break;
            }

            return JsonSerializer.Serialize(list);
        }

        private string Replace(string url, string[][] p)
        {
            foreach (var v in p) url = url.Replace(v[0], v[1]);
            return url;
        }
    }
}
