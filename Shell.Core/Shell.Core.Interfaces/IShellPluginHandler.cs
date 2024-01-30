using System.Reflection;

namespace Shell.Core.Interfaces
{
    public interface IShellPluginHandler
    {
        void SetupDefaultDirectory();
        void SetupDirectory(string dir);
        void SetupPlugin(Assembly assembly);
        void SetupPlugin(string path);
    }
}
