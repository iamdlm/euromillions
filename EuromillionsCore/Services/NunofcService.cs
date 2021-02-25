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
            DrawsDTO drawsDTO = JsonSerializer.Deserialize<DrawsDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            DateTime firstDraw = DateTime.Parse(config.GetSection("FirstDrawDate").Value, new CultureInfo("en-US", true));

            List<Draw> draws = new List<Draw>();

            foreach (DrawDTO drawDTO in drawsDTO.Drawns.Where(x => x.Date >= firstDraw).OrderByDescending(o => o.Date).ToList())
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
