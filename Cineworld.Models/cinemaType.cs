using Cineworld.Models.Helpers;
using System;

namespace Cineworld.Models
{

	public partial class cinemaType : IEquatable<cinemaType>
	{
		public bool Equals(cinemaType other)
		{
			return (this.listing?.CollectionEquals(other?.listing) ?? false)
				&& this.name.SafeEquals(other?.name)
				&& this.root.SafeEquals(other?.root)
				&& this.url.SafeEquals(other?.url)
				&& this.id == other?.id
				&& this.phone.SafeEquals(other?.phone)
				&& this.address.SafeEquals(other?.address)
				&& this.postcode.SafeEquals(other?.postcode);
		}

		public override string ToString() => $"{name}, ({id:D}), {(listing?.Length ?? 0):D} listing(s)";
	}
}
