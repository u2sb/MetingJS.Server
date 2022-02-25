using MetingJS.Server.Models;
using MetingJS.Server.Utils;
using Microsoft.AspNetCore.Mvc;
using static MetingJS.Server.Models.QueryType;

namespace MetingJS.Server.Controllers;

[Route("api/music")]
[Route("api.php")]
[Route("api/meting")]
[ApiController]
public class MetingController : ControllerBase
{
    private readonly Meting _meting;
    private readonly AppSettings _settings;

    public MetingController(Meting meting, AppSettings settings)
    {
        _meting = meting;
        _settings = settings;
    }

    [HttpGet]
    public async Task<ActionResult<string?>> Get([FromQuery] QueryModel query)
    {
        if (string.IsNullOrEmpty(query.Id)) return BadRequest();

        var meting = query.Server.Equals(_settings.DefaultServerProvider)
            ? _meting
            : _meting.SetServer(query.Server);

        switch (query.Type)
        {
            case Lrc:
                return await meting.GetLrcAsync(query.Id);
            case Pic:
                var picUrl = meting.GetPicAsync(query.Id);
                return Redirect(await picUrl ?? "404");
            case QueryType.Url:
                var url = meting.GetUrlAsync(query.Id);
                return Redirect(await url ?? "404");
            default:
                var request = HttpContext.Request;
                var baseUrl = string.IsNullOrEmpty(_settings.Url)
                    ? $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}"
                    : _settings.Url;
                return await meting.SearchAsync(query, baseUrl);
        }
    }
}