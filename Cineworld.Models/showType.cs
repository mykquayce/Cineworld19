using Cineworld.Models.Helpers;
using System;

namespace Cineworld.Models
{

	public partial class showType : IEquatable<showType>
	{
		public bool Equals(showType other)
		{
			return this.time.Equals(other?.time)
				&& this.url.SafeEquals(other?.url)
				&& this.videoType.SafeEquals(other?.videoType)
				&& this.audioType.SafeEquals(other?.audioType)
				&& this.subtitled.SafeEquals(other?.subtitled)
				&& this.sessionType.SafeEquals(other?.sessionType);
		}
	}
}
