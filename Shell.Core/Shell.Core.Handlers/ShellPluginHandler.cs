using Shell.Core.Abstracts;
using Shell.Core.Helpers;
using Shell.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace Shell.Core.Handlers
{
    public sealed class ShellPluginHandler : IShellPluginHandler
    {
        public static IShellPluginHandler Instance = new ShellPluginHandler();
        private Dictionary<Assembly, IShellPlugin> assemblies = new Dictionary<Assembly, IShellPlugin>();

        public ShellPluginHandler()
        {
            //SetupDirectory("Plugins");
        }

        public void SetupDefaultDirectory()
        {
            try
            {
                SetupDirectory("Plugins");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SetupDirectory(string dir)
        {
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var assemblies = Directory.GetFiles("Plugins", "*.dll");
                if (assemblies == null || assemblies.Length == 0)
                    return;
                assemblies.ToList().ForEach(SetupPlugin);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SetupPlugin(Assembly assembly)
        {
            throw new NotImplementedException();
        }

        public void SetupPlugin(string path)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(path);
                if (assembly == null)
                    return;
                var types = assembly.GetTypes();
                types.ToList().ForEach((type) =>
                {
                    if(type.IsClass && type.IsPublic && !type.IsAbstract && type.IsSubclassOf(typeof(ShellPlugin)))
                    {
                        object obj = Activator.CreateInstance(type);
                        if (obj == null)
                            return;
                        var instance = obj as IShellPlugin;
                        if (instance == null)
                            return;
                        if (assemblies.ContainsKey(assembly))
                            return;
                        ShellListenerHandler.Instance.SetupAssembly(assembly);
                        instance.Setup();
                        ShellCommandHandler.Instance.SetupAssembly(assembly);
                        Utils.PrintInfo($"^15Plugin '^3{instance.Name}^15' version '^3[{instance.Version}]^15' by '^3{instance.Developer}^15'");
                        assemblies.Add(assembly, instance);
                        ShellListenerHandler.Instance.Handle("plugin-setup", instance.Name, instance.Developer, instance.Version);
                    }
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
