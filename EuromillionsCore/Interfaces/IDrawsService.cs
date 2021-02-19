using EuromillionsCore.Models;
using System.Collections.Generic;

namespace EuromillionsCore.Interfaces
{
    interface IDrawsService
    {
        Draw Generate();

        Draw Generate(List<Draw> previousDraws);

        bool IsDrawValid(Draw draw);

        bool IsDrawValid(Draw draw, List<Draw> previousDraws);
    }
}
