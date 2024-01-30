using Shell.Core.Interfaces;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shell.Core.Extensions
{
    public static class ShellCommandExtensions
    {
        public static string ShellCommandToJson(this IShellCommand shellCommand)
        {
            return ShellCommandToJson(shellCommand, Formatting.None);
        }
        public static string ShellCommandToJson(this IShellCommand shellCommand, Formatting formatting)
        {
            string json = "";

            Dictionary<string, object> dict = new Dictionary<string, object>()
            {
                { "name", shellCommand.Name },
                { "description", shellCommand.Description },
                { "type", shellCommand.ShellCommandType.ToString() },
                { "aliases", shellCommand.Aliases },
                { "usage", shellCommand.Usage },
            };

            json = JsonConvert.SerializeObject(dict, formatting).Replace("{", "{{").Replace("}", "}}");

            return json;
        }
        public static string ShellCommandToHelp(this IShellCommand shellCommand)
        {
            string help = "";

            help = //$"^3Help ^15for ^5shell command^15 \"^2{shellCommand.Name}^15\":\n" +
                $"[~T~]^15Name: ^7{shellCommand.Name}^15\n" +
                $"[~T~]^15Description: ^7{shellCommand.Description}^15\n" +
                $"[~T~]^15Aliases: ^7{string.Join(", ", shellCommand.Aliases)}^15\n" +
                $"[~T~]^15Type: ^7{shellCommand.ShellCommandType.ToString()}^15\n" +
                $"[~T~]^15Usage: ^7{shellCommand.Usage}^15\n";

            return help.Replace("[~T~]", "    ");
        }
        public static string ShellCommandToUsage(this IShellCommand shellCommand)
        {
            string help = "^15Usage: ^5" + shellCommand.Usage;
            return help;
        }
    }
}