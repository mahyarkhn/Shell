using Shell.Core.Abstracts;
using Shell.Core.Helpers;
using System;
using System.Threading;

namespace Shell.Core.Commands
{
    internal sealed class ExitCommand : ShellCommand
    {
        public override void Setup()
        {
            Name = "exit";
            Aliases.Add("close");
            Description = "close the application";
            Usage = "exit";
        }

        public override void Execute()
        {
            Utils.SmartPrint("^3Closing application . . . ");
            Thread.Sleep(1000);
            Console.WriteLine();
            GC.Collect();
            Environment.Exit(-1);
            GC.Collect();
        }

        public override void Execute(string[] args)
        {
            Execute();
        }
    }
}