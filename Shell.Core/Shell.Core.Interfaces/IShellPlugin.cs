using System;

namespace Shell.Core.Interfaces
{
    interface IShellPlugin
    {
        void Setup();
        string Name { get; }
        string Developer { get; }
        string Version { get; }
    }
}
