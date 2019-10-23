using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Meting4Net.Core;
using MetingJS.Server.Models;
using MMeting = Meting4Net.Core.Meting;
using static MetingJS.Server.Models.QueryType;

namespace MetingJS.Server.Utils
{
	public class Meting : IMeting
	{
		private readonly AppSettings _config;

		public Meting(AppSettings appSettings)
		{
			_config = appSettings;
		}


		public string GetLrc(ServerProvider server, string id)
		{
			var lrc = new MMeting(server) { TryCount = 20 }.LyricObj(id);
			return lrc.lyric;
		}

		public string GetPic(ServerProvider server, string id)
		{
			var meting = new MMeting(server) { TryCount = 10 };
			var pic = meting.PicObj(id, 90);
			var picUrl = pic.url;
			if (!string.IsNullOrEmpty(picUrl) && _config.Replace.Pic != null)
				picUrl = Replace(picUrl, _config.Replace.Pic);
			return picUrl;
		}

		public string GetUrl(ServerProvider server, string id)
		{
			var meting = new MMeting(server) { TryCount = 10 };
			var url = meting.UrlObj(id);
			var urlUrl = url.url;
			if (!string.IsNullOrEmpty(urlUrl) && _config.Replace.Url != null)
				urlUrl = Replace(urlUrl, _config.Replace.Url);
			return urlUrl;
		}

		public string Search(ServerProvider server, QueryType type, string id)
		{
			var meting = new MMeting(server) { TryCount = 10 };
			var list = new List<MusicList>();
			switch (type)
			{
				case Album:
					list = meting.AlbumObj(id).Select(s => new MusicList(s, server, _config)).ToList();
					break;
				case Artist:
					list = meting.ArtistObj(id).Select(s => new MusicList(s, server, _config)).ToList();
					break;
				case PlayList:
					list = meting.PlaylistObj(id).Select(s => new MusicList(s, server, _config)).ToList();
					break;
				case QueryType.Search:
					list = meting.SearchObj(id).Select(s => new MusicList(s, server, _config)).ToList();
					break;
				case Song:
					list = new List<MusicList> { new MusicList(meting.SongObj(id), server, _config) };
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