using Shell.Core.Attributes;
using Shell.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Shell.Core.Handlers
{
    public class ShellListenerHandler : Singleton<ShellListenerHandler>
    {
        Dictionary<MethodInfo, string> registeredMthods = new Dictionary<MethodInfo, string>();

        public ShellListenerHandler()
        {
            SetupAssembly(Assembly.GetExecutingAssembly());
        }
        public void SetupAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract))
            {
                if (type == null) continue;
                foreach (var method in type.GetMethods().Where(x => x.IsStatic))
                {
                    if (method == null) continue;
                    var attr = method.GetCustomAttribute<ShellListenerAttribute>();
                    if (attr == null) continue;
                    registeredMthods.Add(method, attr.Type);
                }
            }
        }

        public bool Handle(string type, params object[] parameters)
        {
            try
            {
                foreach (var kvp in registeredMthods)
                {
                    if (kvp.Value == type)
                    {
                        if (kvp.Key == null)
                            continue;
                        if (kvp.Key.GetParameters().Length == parameters.Length)
                        {
                            bool match = true;
                            var parms = kvp.Key.GetParameters();
                            for (int i = 0; i < parameters.Length; i++)
                            {
                                if (parms[i].ParameterType != parameters[i].GetType())
                                {
                                    match = false;
                                    break;
                                }
                            }
                            if (!match)
                                continue;
                        }
                        else
                            continue;

                        kvp.Key.Invoke(null, parameters);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<T> Handle<T>(string type, params object[] parameters)
        {
            foreach (var kvp in registeredMthods)
            {
                if (kvp.Value == type)
                {
                    if (kvp.Key == null || kvp.Key.ReturnType != typeof(T))
                        continue;
                    if (kvp.Key.GetParameters().Length == parameters.Length)
                    {
                        bool match = true;
                        var parms = kvp.Key.GetParameters();
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            if (parms[i].ParameterType != parameters[i].GetType())
                            {
                                match = false;
                                break;
                            }
                        }
                        if (!match)
                            continue;
                    }
                    else
                        continue;

                    yield return (T)kvp.Key.Invoke(null, parameters);
                }
            }
        }
    }
}
