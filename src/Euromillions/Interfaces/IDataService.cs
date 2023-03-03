using Euromillions.Entities;
using System.Collections.Generic;

namespace Euromillions.Interfaces
{
    interface IDataService
    {
        List<Draw> ReadFile(Type type);

        void SaveFile(List<Draw> draws, Type type);

        List<Draw> UpdateFile(List<Draw> draws, Draw lastDraw, Type type);
    }
}