using EuromillionsCore.Entities;
using System.Collections.Generic;

namespace EuromillionsCore.Interfaces
{
    interface IDataService
    {
        List<Draw> ReadFile(Type type);

        void SaveFile(List<Draw> draws, Type type);

        List<Draw> UpdateFile(List<Draw> draws, Draw lastDraw, Type type);
    }
}