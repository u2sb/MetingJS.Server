using Meting4Net.Core;
using MetingJS.Server.Models;

namespace MetingJS.Server.Utils
{
	public interface IMeting
	{
		string GetLrc(ServerProvider server, string id);

		string GetPic(ServerProvider server, string id);
		string GetUrl(ServerProvider server, string id);

		string Search(ServerProvider server, QueryType type, string id);
	}
}
