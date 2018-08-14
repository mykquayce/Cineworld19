using System.IO;

namespace Cineworld.Services
{
	public interface ISerializationService
	{
		T DeepClone<T>(T obj);
		Stream Serialize<T>(T obj);
		T Deserialize<T>(Stream stream);
	}
}
