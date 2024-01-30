using Shell.Core.Abstracts;
using Shell.Core.Exceptions;
using Shell.Core.Handlers;
using Shell.Core.Helpers;
using System;
using System.Linq;
using Newtonsoft.Json;
using Shell.Core.Extensions;

namespace Shell.Core.Commands
{
    internal sealed class HelpCommand : ShellCommand
    {
        public override void Setup()
        {
            Name = "help";
            Aliases.Add("commands");
            Description = "get a list of available commands or get information for a specified one";
            Usage = "help [-c|--command] [-t|--type ('json'|'json-indented'|'text')]";
        }

        public override void Execute()
        {
            Console.WriteLine();
            Utils.PrintResponse("^3Available Commands:");
            var commands = (ShellCommandHandler.Instance as ShellCommandHandler).GetShellCommands().ToList();
            for (int i = 0; i < commands.Count(); i++)
            {
                if (i % 2 == 0)
                {
                    Utils.SmartPrintLn("\t^7" + commands[i].Name);
                }
                else
                {
                    Utils.SmartPrintLn("\t^6" + commands[i].Name);
                }
            }
            Console.WriteLine();
            Utils.SmartPrintLn("^7To get help for a command use \"^10help <command>^7\" or \"^10<command> <-h|--help|/?>^7\"");
            Utils.SmartPrintLn("^7To see usage of a command use \"^10<command> <-u|--usage>^7\"");
        }

        public override void Execute(string[] args)
        {
            var atd = args.ArgumentsToDict().ToList();

            var cmd = "";
            bool jsonText = false;
            Formatting formatting = Formatting.None;

            

            if (!atd.TryGetArgument("-c|--command", out cmd))
            {
                cmd = atd.First().Key;
            }
            if(atd.TryGetArgument("-t|--type", out var type))
            {
                if (type == "json")
                {
                    jsonText = true;
                    formatting = Formatting.None;
                }
                else if (type == "json-indented" || type == "json-ind" || type == "json-sort")
                {
                    jsonText = true;
                    formatting = Formatting.Indented;
                }
                else if(type == "none")
                {
                    jsonText = false;
                }
            }
            var commands = (ShellCommandHandler.Instance as ShellCommandHandler)[cmd];

            if(commands.Count() != 1)
            {
                var ex = new ShellCommandNotFoundException();
                ex.Data["name"] = args[0];
                throw ex;
            }

            Utils.PrintResponse(string.Format(" ^3Help for \"{0}\":", cmd));
            if (!jsonText)
            {
                Utils.SmartPrintLn(commands.First().ShellCommandToHelp());
            }
            else
            {
                Utils.SmartPrintLn(commands.First().ShellCommandToJson(formatting));
            }
        }
    }

    internal sealed class EchoCommand : ShellCommand
    {
        public override void Execute()
        {
            this.ShellCommandToUsage();
        }

        public override void Execute(string[] args)
        {
            var value = "";
            var type = "";

            if(!args.ArgumentsToDict().TryGetArgument("-v|--value", out value))
            {
                value = args.First().Trim();
            }
            if(!args.ArgumentsToDict().TryGetArgument("-t|--type", out type))// && !args.ArgumentsToDict().TryGetArgument("-sml|--smartlog|--smart-log", out var t))
            {
                type = "smart-log";
            }

            if(type == "smart-log")
            {
                Utils.SmartPrintLn(value);
            }
            else
            {
                Console.WriteLine(value);
            }
        }

        public override void Setup()
        {
            Name = "echo";
            Description = "prints a text";
            Usage = "echo <-v|--value> [-t|--type ('smart-log'|'none')]";
        }
    }
}
