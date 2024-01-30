using Shell.Core.Interfaces;
using System;

namespace Shell.Core.Abstracts
{
    public abstract class ShellPlugin : IShellPlugin
    {
        public virtual string Name { get; protected set; } = "unknown";

        public virtual string Developer { get; protected set; } = "unknown";

        public virtual string Version { get; protected set; } = "0.0";

        public abstract void Setup();
    }
}