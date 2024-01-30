using System;
using System.Reflection;
namespace Shell.Core.Interfaces
{
    public interface IShellCommandHandler
    {
        void SetupAssembly(Assembly assembly);
        void SetupType(Type type);
        bool Handle(string name, params string[] args);
    }
}