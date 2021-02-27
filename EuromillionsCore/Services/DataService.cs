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

        public List<Draw> ReadFile(Entities.Type type)
        {
            string path = string.Empty;

            if (type == Entities.Type.Drawn)
            {
                path = config.GetSection("FilePathDrawn").Value;
            }
            else if(type == Entities.Type.Generated)
            {
                path = config.GetSection("FilePathGenerated").Value;
            }

            if (!File.Exists(path))
            {
                Console.WriteLine("File doesn't exist.");

                return null;
            }

            string json = File.ReadAllText(path);

            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine("File exists but it's empty.");

                return null;
            }

            List<Draw> draws = JsonSerializer.Deserialize<List<Draw>>(json);

            return draws;
        }

        public void SaveFile(List<Draw> draws, Entities.Type type)
        {
            string json = JsonSerializer.Serialize(draws);

            if (type == Entities.Type.Drawn)
            {
                File.WriteAllText(config.GetSection("FilePathDrawn").Value, json);
            }
            else if (type == Entities.Type.Generated)
            {
                File.WriteAllText(config.GetSection("FilePathGenerated").Value, json);
            }

            Console.WriteLine("Past draws list saved.");
        }

        public List<Draw> UpdateFile(List<Draw> draws, Draw lastDraw, Entities.Type type)
        {
            draws.Add(lastDraw);

            SaveFile(draws, type);

            Console.WriteLine("Past draws list updated.");

            return draws;
        }
    }
}
