using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shell.Core.Abstracts;
using Shell.Core.Handlers;
using System.Reflection;
using Shell.Core.Attributes;

namespace Shell
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ShellCommandHandler.Instance.SetupAssembly(Assembly.GetExecutingAssembly());
            ShellListenerHandler.Instance.SetupAssembly(Assembly.GetExecutingAssembly());
            ShellPluginHandler.Instance.SetupDefaultDirectory();
            ShellInputHandler.Instance.StartLoopInput();
        }
    }
}
