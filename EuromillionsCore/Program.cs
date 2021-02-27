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

            previousDraws = dataService.ReadFile(Entities.Type.Drawn);

            if (previousDraws == null)
            {
                await GetAllAndSaveAsync(nunofcService, dataService);
            }
            else
            {
                // Get last draw from API

                Draw lastDraw = await nunofcService.GetLastAsync();

                // Get last draw from past draws list

                Draw lastDrawSaved = previousDraws.OrderByDescending(o => o.Date).FirstOrDefault();

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
                        previousDraws = await GetAllAndSaveAsync(nunofcService, dataService);
                    }
                    else
                    {
                        // Update past draws list with last draw

                        previousDraws = dataService.UpdateFile(previousDraws, lastDraw, Entities.Type.Drawn);
                    }
                }
            }

            // Generate number of keys according to appsettings

            List<Draw> generatedDraws = drawsService.Generate(previousDraws);

            // Get previous generated draws

            List<Draw> previousGeneratedDraws = dataService.ReadFile(Entities.Type.Generated);

            if (previousGeneratedDraws != null && previousGeneratedDraws.Any())
            {
                List<Draw> stillValidGeneratedDraws = new List<Draw>();

                // Get last generated draw date

                Draw previousGeneratedLastDraw = previousGeneratedDraws.OrderByDescending(o => o.Date).ToList().FirstOrDefault();

                // Get last generated draws by date of last generated draw

                List<Draw> previousGeneratedDrawsByDate = previousGeneratedDraws.Where(w => w.Date == previousGeneratedLastDraw.Date).ToList();

                foreach (Draw draw in previousGeneratedDrawsByDate)
                {
                    // Add draw to temp generated list if valid

                    if (drawsService.IsDrawValid(draw, previousDraws))
                    {
                        stillValidGeneratedDraws.Add(draw);
                    }
                }

                if (stillValidGeneratedDraws.Any())
                {
                    // Replace generated draws with previous generated draws that are still valid

                    int missingDraws = generatedDraws.Count() - stillValidGeneratedDraws.Count();

                    if (missingDraws > 0)
                    {
                        Random random = new Random();

                        for (int i = 0; i < missingDraws; i++)
                        {
                            int index = random.Next(missingDraws);

                            stillValidGeneratedDraws.Add(generatedDraws[index]);
                        }
                    }

                    generatedDraws = stillValidGeneratedDraws;
                }
            }

            // Send keys by email

            // mailService.Send(generatedDraws);

            // Save generated keys to file

            dataService.SaveFile(generatedDraws, Entities.Type.Generated);
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
