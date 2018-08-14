using Cineworld.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cineworld.Models
{
	public partial class cinemasType : IEquatable<cinemasType>
	{
		public bool Equals(cinemasType other)
		{
			return this.cinema?.CollectionEquals(other?.cinema) ?? false;
		}

		public override string ToString()
			=> string.Join(Environment.NewLine, ToStrings());

		public IEnumerable<string> ToStrings()
		{
			foreach (var cinema in from c in this.cinema
								   orderby c.name
								   select c)
			{
				yield return cinema.name;

				foreach (var (film, show) in from f in cinema.listing
											 from s in f.shows
											 orderby s.time, f.title
											 select (f, s))
				{
					yield return $@"{show.time:yyyy-MM-dd HH\:mm} : {film.title}";
				}
			}
		}
	}
}
