using Cineworld.Models;
using System.Collections.Generic;

namespace Cineworld.Services
{
    public interface IFilterService
    {
        IEnumerable<cinemasType> Filter(cinemasType cinemas);
    }
}
