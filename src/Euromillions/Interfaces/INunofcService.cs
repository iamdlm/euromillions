using Euromillions.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Euromillions.Interfaces
{
    interface INunofcService
    {
        Task<Draw> GetLastAsync();

        Task<List<Draw>> GetAllAsync();
    }
}