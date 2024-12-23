using System;
using System.Collections.Generic;

namespace InputModels
{
    public abstract class Input
    {
        public string Type { get; set; }
    }

    public class TextInput : Input
    {
        public string Content { get; set; }

        public TextInput()
        {
            Type = "Text";
        }
    }

    public class CommandInput : Input
    {
        public string Command { get; set; }
        public string[] Parameters { get; set; }

        public CommandInput()
        {
            Type = "Command";
        }
    }

    public class CoordinatesInput : Input
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public CoordinatesInput()
        {
            Type = "Coordinates";
        }
    }

    public static class EnvVariablesManager
    {
        public static string GetEnvVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? "Not Defined";
        }

        public static void LoadConfiguration()
        {
            var someApiKey = GetEnvVariable("API_KEY");
            var someOtherConfig = GetEnvVariable("OTHER_CONFIG");

            Console.WriteLine($"API_KEY: {someApiKey}");
            Console.WriteLine($"OTHER_CONFIG: {someOtherConfig}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TextInput textInput = new TextInput { Content = "Hello, world!" };
            CommandInput commandInput = new CommandInput { Command = "echo", Parameters = new string[] { "Hello, command!" } };
            CoordinatesInput coordinatesInput = new CoordinatesInput { Latitude = 34.0522, Longitude = -118.2437 };

            List<Input> inputs = new List<Input> { textInput, commandInput, coordinatesInput };

            foreach (var input in inputs)
            {
                Console.WriteLine($"Processing {input.Type} input.");
            }

            EnvVariablesManager.LoadConfiguration();
        }
    }
}