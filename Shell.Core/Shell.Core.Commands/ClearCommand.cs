using Shell.Core.Abstracts;
using System;

namespace Shell.Core.Commands
{
    internal sealed class ClearCommand : ShellCommand
    {
        public override void Setup()
        {
            Name = "clear";
            Aliases.Add("cls");
            Description = "clear console screen";
            Usage = "clear";
        }

        public override void Execute()
        {
            Console.Clear();
        }

        public override void Execute(string[] args)
        {
            Execute();
        }
    }
}
