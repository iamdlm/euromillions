using EuroMillionsAI.DTOs;
using EuroMillionsAI.Models;
using EuromillionsCore.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EuroMillionsAI.Services
{
    // Source: https://nunofcguerreiro.com/blog/euro-milhoes-api
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

            return Convert(json);
        }

        private List<Draw> Convert(string json)
        {
            DrawsDTO drawsAll = JsonSerializer.Deserialize<DrawsDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            DateTime firstDraw = DateTime.Parse(config.GetSection("FirstDraw").Value, new CultureInfo("en-US", true));

            List<DrawDTO> drawsDTO = drawsAll.Drawns.Where(x => x.Date >= firstDraw).ToList();

            List<Draw> draws = new List<Draw>();

            foreach (DrawDTO draw in drawsDTO.OrderByDescending(o => o.Date))
            {
                draws.Add(new Draw
                {
                    Date = draw.Date,
                    Numbers = draw.Balls.Split(' ').Select(Int32.Parse).ToArray(),
                    Stars = draw.Stars.Split(' ').Select(Int32.Parse).ToArray()
                });
            }

            return draws;
        }
    }
}
