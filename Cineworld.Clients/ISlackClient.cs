using System.Threading.Tasks;

namespace Cineworld.Clients
{
	public interface ISlackClient
    {
		Task<bool> SendMessageAsync(string message);
    }
}
