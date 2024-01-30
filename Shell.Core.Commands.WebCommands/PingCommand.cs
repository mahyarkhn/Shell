using Shell.Core.Abstracts;
using Shell.Core.Helpers;
using Shell.Core.Extensions;
using System;
using System.Diagnostics;
using System.Net;
using System.Linq;

namespace Shell.Core.Commands.WebCommands
{
    public class PingCommand : ShellCommand
    {
        string[] defaultPingAddresses = { "https://google.com/", "https://soft98.ir/", "https://yasdl.com/", "https://sarzamindownload.com/", "https://shatelland.com/" };


        public override void Execute()
        {
            Utils.SmartPrintLn("^2Pinging default addresses^15: ");
            defaultPingAddresses.ToList().ForEach((addr) =>
            {
                if (addr != null)
                    Execute(new[] { addr });
            });
        }

        public override void Execute(string[] args)
        {
            var address = "";
            //var ips = "";
            var dict = args.ArgumentsToDict();
            if(!dict.TryGetArgument("-s|--source", out address))// && !dict.TryGetArgument("-i|--ip", out ips))
            {
                address = args[0];
            }
            if(string.IsNullOrWhiteSpace(address))
            {
                Utils.SmartPrintLn("^12Address is null, please enter it correctly");
                return;
            }
            Utils.SmartPrintLn(string.Format("  ^8Pinging host ^15: {0}", address));
            Stopwatch stopwatch = new Stopwatch();

            HttpWebResponse hr = null;
            try
            {
                stopwatch = new Stopwatch();
                HttpWebRequest httpWeb = WebRequest.CreateHttp(address);
                httpWeb.Timeout = 5000;
                //httpWeb.Method = "POST";
                stopwatch.Start();
                var r = httpWeb.GetResponse();
                stopwatch.Stop();
                hr = r as HttpWebResponse;

                int c = 0;
                if (hr.StatusCode == HttpStatusCode.OK)
                    c = 2;
                else
                    c = 12;

                Utils.SmartPrintLn($"    ^3{address} ^15|^{c} {hr.StatusCode} ^15|^3 {hr.Server} ^15|^{c} {stopwatch.ElapsedMilliseconds} ^15ms");
                stopwatch.Reset();
            }
            catch (Exception)
            {
                if (hr != null)
                {
                    Utils.SmartPrintLn("\n    ^12Connection Refused: ^15" + address + ":");
                    Utils.SmartPrintLn("    ^3Code: ^14" + hr.StatusCode);
                    Utils.SmartPrintLn("    ^3Description: ^14" + hr.StatusDescription + "\n");
                }
                else
                {
                    Utils.SmartPrintLn("\n    ^12Connection Refused: ^15" + address);
                }
                //throw;
            }

            //if(!string.IsNullOrWhiteSpace(addresses))
            //{

            //}
            Console.WriteLine();
        }

        public override void Setup()
        {
            Name = "ping";
            Usage = "ping | ping [-s|--source]"; /* | ping [-i|--ip]*/
            Description = "Ping pages";
        }
    }
}
