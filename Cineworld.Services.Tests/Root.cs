using System;
using System.Xml.Serialization;

namespace Cineworld.Services.Tests
{
	[Serializable]
	[XmlRoot]
	public class Root
	{
		[XmlAttribute]
		public string String { get; set; }

		[XmlAttribute]
		public int Int32 { get; set; }
	}
}
