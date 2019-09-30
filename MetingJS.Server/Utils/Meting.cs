using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Meting4Net.Core;
using MetingJS.Server.Models;
using static MetingJS.Server.Models.AppSettings;
using static MetingJS.Server.Models.QueryType;

namespace MetingJS.Server.Utils
{
	public class Meting : IMeting
	{
		private readonly Meting4Net.Core.Meting _meting;

		public Meting()
		{
			_meting = new Meting4Net.Core.Meting
			{
				TryCount = 12
			};
		}

		public string GetLrc(ServerProvider server, string id)
		{
//			_meting.Server = server;
//			var lrc = _meting.LyricObj(id);
			var lrc = new Meting4Net.Core.Meting(server) {TryCount = 20}.LyricObj(id);
			return lrc.lyric;
		}

		public string GetPic(ServerProvider server, string id)
		{
			_meting.Server = server;
			var pic = _meting.PicObj(id, 90);
			var picUrl = pic.url;
			if (!string.IsNullOrEmpty(picUrl) && Config.Replace.Pic != null)
				picUrl = Replace(picUrl, Config.Replace.Pic);
			return picUrl;
		}

		public string GetUrl(ServerProvider server, string id)
		{
			_meting.Server = server;
			var url = _meting.UrlObj(id);
			var urlUrl = url.url;
			if (!string.IsNullOrEmpty(urlUrl) && Config.Replace.Url != null)
				urlUrl = Replace(urlUrl, Config.Replace.Url);
			return urlUrl;
		}

		public string Search(ServerProvider server, QueryType type, string id)
		{
			_meting.Server = server;
			var list = new List<MusicList>();
			switch (type)
			{
				case Album:
					list = _meting.AlbumObj(id).Select(s => new MusicList(s, server)).ToList();
					break;
				case Artist:
					list = _meting.ArtistObj(id).Select(s => new MusicList(s, server)).ToList();
					break;
				case PlayList:
					list = _meting.PlaylistObj(id).Select(s => new MusicList(s, server)).ToList();
					break;
				case QueryType.Search:
					list = _meting.SearchObj(id).Select(s => new MusicList(s, server)).ToList();
					break;
				case Song:
					list = new List<MusicList> {new MusicList(_meting.SongObj(id), server)};
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