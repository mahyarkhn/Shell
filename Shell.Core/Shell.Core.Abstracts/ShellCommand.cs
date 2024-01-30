using Shell.Core.Enums;
using Shell.Core.Interfaces;
using System.Collections.Generic;

namespace Shell.Core.Abstracts
{
    public abstract class ShellCommand : IShellCommand
    {
        public virtual string Name { get; protected set; } = "undefined";
        public virtual string Description { get; protected set; } = "no-description-available";
        public virtual string Usage { get; protected set; } = "";
        public virtual List<string> Aliases { get; protected set; } = new List<string>();
        public virtual ShellCommandType ShellCommandType { get; protected set; } = ShellCommandType.Release;
        public abstract void Setup();
        public abstract void Execute();
        public abstract void Execute(string[] args);
    }
}