using EuromillionsCore.Interfaces;
using EuromillionsCore.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EuromillionsCore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Init Startup

            var startup = new Startup();

            // Request services instances from the service pipeline
            
            IDataService dataService = startup.Provider.GetRequiredService<IDataService>();
            INunofcService nunofcService = startup.Provider.GetRequiredService<INunofcService>();
            IDrawsService drawsService = startup.Provider.GetRequiredService<IDrawsService>();

            // List of past draws

            List<Draw> draws = new List<Draw>();

            // Get past draws from file

            draws = dataService.ReadFile();

            if (draws == null)
            {
                // Get past draws from API 

                draws = await nunofcService.GetAllAsync();

                if (draws == null)
                {
                    Console.WriteLine("Failed to get all draws.");

                    return;
                }

                // Save past draws to file

                dataService.SaveFile(draws);
            }
            else
            {
                // Get last draw from API

                Draw lastDraw = await nunofcService.GetLastAsync();

                // Get last draw from past draws list

                Draw lastPastDraw = draws.OrderByDescending(o => o.Date).FirstOrDefault();

                if (lastDraw.Date == lastPastDraw.Date)
                {
                    Console.WriteLine("Past draws list already updated.");
                }
                else
                {
                    // Update past draws list with last draw

                    draws = dataService.UpdateFile(draws, lastDraw);
                }
            }

            Draw draw = drawsService.Generate();
        }
    }
}
