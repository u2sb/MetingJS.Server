using System.Text.Json;
using System.Text.Json.Serialization;
using Meting4Net.Core;
using Meting4Net.Core.Models.Standard;
using static MetingJS.Server.Models.AppSettings;

namespace MetingJS.Server.Models
{
	public class MusicList
	{
		public MusicList()
		{
		}

		public MusicList(Music_search_item aItem, ServerProvider server)
		{
			Title = aItem.name;
			Author = string.Join(" / ", aItem.artist);
			Url = $"{Config.Url}?server={server:G}&type=url&id={aItem.url_id}";
			Pic = $"{Config.Url}?server={server:G}&type=pic&id={aItem.pic_id}";
			Lrc = $"{Config.Url}?server={server:G}&type=lrc&id={aItem.lyric_id}";
		}

		[JsonPropertyName("title")] public string Title { get; set; }
		[JsonPropertyName("author")] public string Author { get; set; }
		[JsonPropertyName("url")] public string Url { get; set; }
		[JsonPropertyName("pic")] public string Pic { get; set; }
		[JsonPropertyName("lrc")] public string Lrc { get; set; }
	}
}