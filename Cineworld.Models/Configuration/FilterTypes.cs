﻿using System;

namespace Cineworld.Models.Configuration
{
	[Flags]
	public enum FilterTypes : byte
	{
		None = 0,
		CinemaId = 1,
		DayOfWeek = 2,
		Title = 4,
	}
}
