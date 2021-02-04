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
        public Task<Draw> GetLastAsync();
        public Task<List<Draw>> GetAllAsync();
    }
}