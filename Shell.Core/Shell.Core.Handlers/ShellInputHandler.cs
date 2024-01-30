using Shell.Core.Extensions;
using Shell.Core.Helpers;
using Shell.Core.Interfaces;
using System;
using System.Linq;

namespace Shell.Core.Handlers
{
    public sealed class ShellInputHandler : IShellInputHandler
    {
        public static IShellInputHandler Instance = new ShellInputHandler();

        public void StartLoopInput()
        {
            var cl = Console.CursorLeft;
            var ct = Console.CursorTop;
            Console.SetCursorPosition(Console.WindowWidth / 2 - "Shell".Length / 2, 0);
            Utils.SmartPrint("^12Shell");
            Console.SetCursorPosition(cl, ct + 1);

            while(true)
            {
                Console.Title = "Shell";
                Utils.PrintPrefix();
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                //var args = input.Split(' ');
                Console.Title = Console.Title + " | " + input;
                var sorted = input.ShellCommandLineToArray();
                ShellCommandHandler.Instance.Handle(sorted.First(), sorted.Skip(1).ToArray());
            }
        }        
    }
}