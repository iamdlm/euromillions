using EuroMillionsAI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EuromillionsCore.Interfaces
{
    interface IDataService
    {
        List<Draw> ReadFile();

        void SaveFile(List<Draw> draws);

        void UpdateFile(List<Draw> draws, Draw lastDraw);
    }
}