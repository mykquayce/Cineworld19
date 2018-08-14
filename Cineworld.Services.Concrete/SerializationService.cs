using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Cineworld.Services.Concrete
{
	public class SerializationService : ISerializationService
	{
		private static readonly object _lock = new object();
		private static readonly IDictionary<Type, XmlSerializer> _cache = new Dictionary<Type, XmlSerializer>();

		public T DeepClone<T>(T obj)
		{
			using (var stream = Serialize(obj))
			{
				return Deserialize<T>(stream);
			}
		}

		public T Deserialize<T>(Stream stream)
			=> (T)this[typeof(T)].Deserialize(stream);

		/// <summary>
		/// Deserializes a stream to an object.  Doesn't dispose of the stream
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns>T</returns>
		public Stream Serialize<T>(T obj)
		{
			var stream = new MemoryStream();

			var serializer = this[typeof(T)];

			serializer.Serialize(stream, obj);

			stream.Position = 0L;

			return stream;
		}

		private XmlSerializer this[Type key]
		{
			get
			{
				lock (_lock)
				{
					if (!_cache.ContainsKey(key))
					{
						var serializer = new XmlSerializer(key);

						_cache.Add(key, serializer);
					}
				}

				return _cache[key];
			}
		}
	}
}
