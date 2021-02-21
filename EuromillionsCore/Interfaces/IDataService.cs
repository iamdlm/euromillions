using EuromillionsCore.Models;
using System.Collections.Generic;

namespace EuromillionsCore.Interfaces
{
    interface IDataService
    {
        List<Draw> ReadFile();

        void SaveFile(List<Draw> draws);

        List<Draw> UpdateFile(List<Draw> draws, Draw lastDraw);
    }
}