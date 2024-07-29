using System;
using System.IO;
using System.Text.Json;

namespace AmazonAutomation.Config
{
    public class ConfigProvider
    {
        public string BaseUrl { get; set; }
        public string SearchTerm { get; set; }
        public int MaxPrice { get; set; }
        public string MinMemory { get; set; }
        public int MinRating { get; set; }
        public int MaxReviewCount { get; set; }
        public string ScreenshotPath { get; set; }
        public string Email {get; set;}
        public string Password {get; set;}

        public static ConfigProvider LoadConfig(string filePath)
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);
                ConfigProvider config = JsonSerializer.Deserialize<ConfigProvider>(jsonString);
                return config;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading config file: {ex.Message}");
                throw;
            }
        }
    }
}
