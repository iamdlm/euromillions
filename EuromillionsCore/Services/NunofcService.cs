using EuromillionsCore.DTOs;
using EuromillionsCore.Interfaces;
using EuromillionsCore.Entities;
using EuromillionsCore.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Globalization;

namespace EuromillionsCore.Services
{
    public class NunofcService : INunofcService
    {
        IConfiguration config;

        public NunofcService(IConfiguration _config)
        {
            this.config = _config;
        }

        public async Task<Draw> GetLastAsync()
        {
            List<Draw> draws = await GetAsync(config.GetSection("NunofcAPIConfiguration:ApiUrlLast").Value);

            return draws.FirstOrDefault();
        }

        public async Task<List<Draw>> GetAllAsync()
        {
            return await GetAsync(config.GetSection("NunofcAPIConfiguration:ApiUrlAll").Value);
        }

        private async Task<List<Draw>> GetAsync(string url)
        {
            Console.WriteLine($"Fetching draws from {url}");

            string json = await HttpClientService.GetAsync(url);

            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine($"Failed request to {url}");

                return null;
            }

            return ConvertJsonToDraw(json);
        }

        private List<Draw> ConvertJsonToDraw(string json)
        {
            List<Draw> draws = new List<Draw>();
            List<DrawDTO> drawsDTO = new List<DrawDTO>();
            
            DrawsDTO drawsAll = JsonSerializer.Deserialize<DrawsDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            bool useAllDraws = true;

            try
            {
                useAllDraws = Convert.ToBoolean(config.GetSection("UseAllDraws").Value);
            }
            catch { }

            if (useAllDraws)
            {
                drawsDTO = drawsAll.Drawns.OrderByDescending(o => o.Date).ToList();
            }
            else
            {
                DateTime firstDraw = DateTime.Parse(config.GetSection("NewRulesFirstDrawDate").Value, new CultureInfo("en-US", true));

                drawsDTO = drawsAll.Drawns.Where(x => x.Date >= firstDraw).OrderByDescending(o => o.Date).ToList();                
            }
            
            foreach (DrawDTO drawDTO in drawsDTO)
            {
                draws.Add(new Draw
                {
                    Date = drawDTO.Date,
                    Numbers = drawDTO.Balls.Split(' ').Select(Int32.Parse).ToArray(),
                    Stars = drawDTO.Stars.Split(' ').Select(Int32.Parse).ToArray()
                });
            }

            return draws;
        }
    }
}
