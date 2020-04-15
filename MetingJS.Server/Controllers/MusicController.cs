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
        private readonly Meting _meting;
        private readonly AppSettings _settings;

        public MusicController(Meting meting, AppSettings settings)
        {
            _meting = meting;
            _settings = settings;
        }

        [HttpGet]
        public ActionResult<string> Get([FromQuery] QueryModel query)
        {

            if (string.IsNullOrEmpty(query.Id)) return BadRequest();

            var meting = query.Server.Equals(_settings.DefaultServerProvider)
                    ? _meting
                    : _meting.SetServer(query.Server);

            switch (query.Type)
            {
                case Lrc:
                    return meting.GetLrc(query.Id);
                case Pic:
                    var picUrl = meting.GetPic(query.Id);
                    return Redirect(picUrl);
                case QueryType.Url:
                    var url = meting.GetUrl(query.Id);
                    return Redirect(url);
                default:
                    var request = HttpContext.Request;
                    var baseUrl = string.IsNullOrEmpty(_settings.Url)
                            ? $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}"
                            : _settings.Url;
                    return meting.Search(query, baseUrl);
            }
        }
    }
}
