using Cineworld.Models;

namespace Cineworld.Services
{
    public interface IFilterService
    {
        cinemasType Filter(cinemasType before);
    }
}
