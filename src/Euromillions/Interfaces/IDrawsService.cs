﻿using Euromillions.Entities;
using System.Collections.Generic;

namespace Euromillions.Interfaces
{
    interface IDrawsService
    {
        List<Draw> Generate();

        List<Draw> Generate(List<Draw> pastDraws);

        bool IsDrawValid(Draw draw);

        bool IsDrawValid(Draw draw, List<Draw> pastDraws);

        int EvaluatePrize(Draw draw, Draw prize);
    }
}
