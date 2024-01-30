using Shell.Core.Enums;
using System.Collections.Generic;

namespace Shell.Core.Interfaces
{
    public interface IShellCommand
    {
        string Name { get; }
        string Description { get; }
        List<string> Aliases { get; }
        string Usage { get; }
        ShellCommandType ShellCommandType { get; }
        void Setup();
        void Execute();
        void Execute(string[] args);
    }
}