using EuroMillionsAI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuromillionsCore.Interfaces
{
    interface INunofcService
    {
        Task<Draw> GetLastAsync();
        Task<List<Draw>> GetAllAsync();
    }
}