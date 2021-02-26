using EuromillionsCore.Interfaces;
using EuromillionsCore.Entities;
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
            IMailService mailService = startup.Provider.GetRequiredService<IMailService>();

            // List of past draws

            List<Draw> previousDraws = new List<Draw>();

            // Get past draws from file

            previousDraws = dataService.ReadFile();

            if (previousDraws == null)
            {
                // Get past draws from API 

                previousDraws = await nunofcService.GetAllAsync();

                if (previousDraws == null)
                {
                    Console.WriteLine("Failed to get all draws.");

                    return;
                }

                // Save past draws to file

                dataService.SaveFile(previousDraws);
            }
            else
            {
                // Get last draw from API

                Draw lastDraw = await nunofcService.GetLastAsync();

                // Get last draw from past draws list

                Draw lastPastDraw = previousDraws.OrderByDescending(o => o.Date).FirstOrDefault();

                if (lastDraw.Date == lastPastDraw.Date)
                {
                    Console.WriteLine("Past draws list already updated.");
                }
                else
                {
                    // Update past draws list with last draw

                    previousDraws = dataService.UpdateFile(previousDraws, lastDraw);
                }
            }

            List<Draw> genDraws = drawsService.Generate(previousDraws);

            Console.WriteLine("New keys generated.");

            mailService.Send(genDraws);

            Console.WriteLine("New keys sent by email.");
        }
    }
}
