using EuromillionsCore.Models;
using System.Collections.Generic;

namespace EuromillionsCore.Interfaces
{
    interface IDrawsService
    {
        List<Draw> Generate();

        List<Draw> Generate(List<Draw> previousDraws);

        bool IsDrawValid(Draw draw);

        bool IsDrawValid(Draw draw, List<Draw> previousDraws);
    }
}
