using Euromillions.Interfaces;
using Euromillions.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Euromillions
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

            List<Draw> pastDraws = new List<Draw>();

            // Get past draws from file

            pastDraws = dataService.ReadFile(Entities.Type.Drawn);

            if (pastDraws == null)
            {
                await GetAllAndSaveAsync(nunofcService, dataService);
            }
            else
            {
                // Get last draw from API

                Draw lastDraw = await nunofcService.GetLastAsync();

                // Get last draw from past draws list

                Draw lastDrawSaved = pastDraws.OrderByDescending(o => o.Date).FirstOrDefault();

                if (lastDraw.Date == lastDrawSaved.Date)
                {
                    Console.WriteLine("Past draws list already updated.");
                }
                else
                {
                    // Days difference between today and last draw in file

                    TimeSpan daysDif = lastDraw.Date - lastDrawSaved.Date;

                    // Get all draws if last draw saved was more than 3 or 4 days ago (Friday or Tuesday respectively)

                    if (lastDraw.Date.DayOfWeek == DayOfWeek.Friday && daysDif.Days > 3 ||
                        lastDraw.Date.DayOfWeek == DayOfWeek.Tuesday && daysDif.Days > 4)
                    {
                        pastDraws = await GetAllAndSaveAsync(nunofcService, dataService);
                    }
                    else
                    {
                        // Update past draws list with last draw

                        pastDraws = dataService.UpdateFile(pastDraws, lastDraw, Entities.Type.Drawn);
                    }
                }
            }

            // Generate number of keys according to appsettings

            List<Draw> newDraws = drawsService.Generate(pastDraws);

            // Get past generated draws

            List<Draw> pastGeneratedDraws = dataService.ReadFile(Entities.Type.Generated);

            if (pastGeneratedDraws != null && pastGeneratedDraws.Any())
            {
                List<Draw> stillValidGeneratedDraws = new List<Draw>();

                // Get last generated draw

                Draw lastGeneratedDraw = pastGeneratedDraws.OrderByDescending(o => o.Date).ToList().FirstOrDefault();

                foreach (Draw draw in pastGeneratedDraws.Where(w => w.Date == lastGeneratedDraw.Date).ToList())
                {
                    // Add draw to temp generated list if valid

                    if (drawsService.IsDrawValid(draw, pastDraws))
                    {
                        stillValidGeneratedDraws.Add(draw);
                    }
                }

                if (stillValidGeneratedDraws.Any())
                {
                    // Replace new draws with past generated draws that are still valid

                    int missingDraws = newDraws.Count() - stillValidGeneratedDraws.Count();

                    if (missingDraws > 0)
                    {
                        Random random = new Random();

                        for (int i = 0; i < missingDraws; i++)
                        {
                            int index = random.Next(missingDraws);

                            stillValidGeneratedDraws.Add(newDraws[index]);
                        }
                    }

                    newDraws = stillValidGeneratedDraws;
                }
            }

            // Send keys by email

            mailService.Send(newDraws);

            // Save generated keys to file

            dataService.SaveFile(newDraws, Entities.Type.Generated);
        }

        private static async Task<List<Draw>> GetAllAndSaveAsync(INunofcService nunofcService, IDataService dataService)
        {
            // Get past draws from API

            List<Draw> draws = await nunofcService.GetAllAsync();

            if (draws == null)
            {
                Console.WriteLine("Failed to get all draws.");

                return draws;
            }

            // Save past draws to file

            dataService.SaveFile(draws, Entities.Type.Drawn);

            return draws;
        }
    }
}
