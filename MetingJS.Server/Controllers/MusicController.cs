using System;
using Meting4Net.Core;
using MetingJS.Server.Models;
using MetingJS.Server.Utils;
using Microsoft.AspNetCore.Mvc;
using static MetingJS.Server.Models.QueryType;

namespace MetingJS.Server.Controllers
{
	[Route("api/music")]
    [Route("api.php")]
	[ApiController]
	public class MusicController : ControllerBase
	{
		private readonly IMeting _meting;

		public MusicController(IMeting meting)
		{
			_meting = meting;
		}
		
		[HttpGet]
		public string Get()
		{
			var query = HttpContext.Request.Query;
			var server = query["server"];
			var type = query["type"];
			var id = query["id"];
			ServerProvider serverProvider;
			QueryType queryType;

			if (string.IsNullOrEmpty(id))
			{
				Response.StatusCode = 400;
				return null;
			}
			if (Enum.TryParse(typeof(ServerProvider), server,true, out var o))
			{
				serverProvider = (ServerProvider) o;
			}
			else
			{
				Response.StatusCode = 400;
				return null;
			}

			if (Enum.TryParse(typeof(QueryType), type, true, out var t))
			{
				queryType = (QueryType) t;
			}
			else
			{
				Response.StatusCode = 400;
				return null;
			}

			switch (queryType)
			{
				case Lrc:
					return _meting.GetLrc(serverProvider, id);
				case Pic:
					var picUrl = _meting.GetPic(serverProvider, id);
					Response.Headers.Add("Location",picUrl);
					Response.StatusCode = 302;
					break;
				case QueryType.Url:
					var url = _meting.GetUrl(serverProvider, id);
					Response.Headers.Add("Location", url);
					Response.StatusCode = 302;
					break;
				default:
					return _meting.Search(serverProvider, queryType, id);
			}

			return null;
		}
	}
}