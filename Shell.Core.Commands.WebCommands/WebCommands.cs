using Shell.Core.Abstracts;
using Shell.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Shell.Core.Commands.WebCommands
{
    public class WebCommands : ShellPlugin
    {
        public WebCommands()
        {
        }

        public override void Setup()
        {
            Name = "WebCommands";
            Version = "1.0.0.0";
            Developer = "Mahyar";
        }
    }
}
