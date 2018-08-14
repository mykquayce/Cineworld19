using Cineworld.Models.Helpers;
using System;

namespace Cineworld.Models
{

	public partial class filmType : IEquatable<filmType>
	{
		public bool Equals(filmType other)
		{
			return (this.shows?.CollectionEquals(other?.shows) ?? false)
				&& this.title.SafeEquals(other?.title)
				&& this.rating.SafeEquals(other?.rating)
				&& this.url.SafeEquals(other?.url)
				&& (this.edi == other?.edi)
				&& this.release.SafeEquals(other?.release);
		}
	}
}
