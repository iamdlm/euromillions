using Euromillions.Entities;
using System.Collections.Generic;

namespace Euromillions.Interfaces
{
    interface IMailService
    {
        void Send(List<Draw> draws);
    }
}