using EuroMillionsAI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EuromillionsCore.Interfaces
{
    interface IDrawsService
    {
        Draw Generate();

        bool IsDrawValid(Draw draw);
    }
}
