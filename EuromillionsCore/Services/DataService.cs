using EuromillionsCore.Interfaces;
using EuromillionsCore.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EuromillionsCore.Services
{
    public class DataService : IDataService
    {
        IConfiguration config;

        public DataService(IConfiguration _config)
        {
            this.config = _config;

            // Create folder for data dumps

            Directory.CreateDirectory(config.GetSection("FolderPath").Value);
        }

        public List<Draw> ReadFile()
        {
            string json = string.Empty;

            if (!File.Exists(config.GetSection("FilePath").Value))
            {
                Console.WriteLine("File doesn't exist.");

                return null;
            }

            json = File.ReadAllText(config.GetSection("FilePath").Value);

            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine("File exists but it's empty.");

                return null;
            }

            List<Draw> draws = JsonSerializer.Deserialize<List<Draw>>(json);

            return draws;
        }

        public void SaveFile(List<Draw> draws)
        {
            string json = JsonSerializer.Serialize(draws);

            File.WriteAllText(config.GetSection("FilePath").Value, json);

            Console.WriteLine("Past draws list saved.");
        }

        public List<Draw> UpdateFile(List<Draw> draws, Draw lastDraw)
        {
            draws.Add(lastDraw);

            SaveFile(draws);

            Console.WriteLine("Past draws list updated.");

            return draws;
        }
    }
}
