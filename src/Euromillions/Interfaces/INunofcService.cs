using EuromillionsCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EuromillionsCore.Interfaces
{
    interface INunofcService
    {
        Task<Draw> GetLastAsync();

        Task<List<Draw>> GetAllAsync();
    }
}