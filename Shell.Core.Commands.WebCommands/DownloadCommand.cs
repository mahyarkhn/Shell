using Shell.Core.Abstracts;
using Shell.Core.Helpers;
using Shell.Core.Extensions;
using System;
using System.Net;
using System.ComponentModel;
using System.IO;

namespace Shell.Core.Commands.WebCommands
{
    public class DownloadCommand : ShellCommand
    {
        public override void Execute()
        {
            Utils.SmartPrintLn(this.ShellCommandToUsage());
        }

        public override async void Execute(string[] args)
        {
            var source = "";
            var output = "";
            var type = ""; // (text | string) | file
            var dltype = ""; // async / async-task / none
            var dict = args.ArgumentsToDict();
            if(!dict.TryGetArgument("-s|--source", out source))
            {
                source = args[0];
            }
            if (!dict.TryGetArgument("-o|--output", out output))
            {
                output = source;
            }
            if (!dict.TryGetArgument("-dt|--dltype", out dltype))
            {
                dltype = "none";
            }
            if (!dict.TryGetArgument("-t|--type", out type))
            {
                type = "file";
            }

            if(string.IsNullOrWhiteSpace(source))
            {
                Utils.SmartPrintLn("^12Sourec is null, please enter it correctly");
                return;
            }
            if (string.IsNullOrWhiteSpace(output))
            {
                Utils.SmartPrintLn("^12Output is null, please enter it correctly");
                return;
            }
            output = "Downloads\\" + output;
            if (!Directory.Exists("Downloads"))
                Directory.CreateDirectory("Downloads");

            try
            {
                var cl = Console.CursorLeft;
                var ct = Console.CursorTop;
                bool ended = false;
                BackgroundWorker bg = new BackgroundWorker();
                bg.WorkerReportsProgress = true;
                bg.ReportProgress(100);
                var nl = 0;
                var ll = 103;
                bg.DoWork += (s, e) =>
                {
                    if(type != "file" && type != "string" && type != "text")
                    {
                        Utils.PrintError("Invalid Type");
                        bg.CancelAsync();
                        return;
                    }

                    Console.SetCursorPosition(0, ct + 1);
                    Console.Write("[");
                    Console.SetCursorPosition(101, ct + 1);
                    Console.Write("] %0\n");

                    WebClient webClient = new WebClient();
                    //webClient.DownloadProgressChanged += (_s, _e) =>
                    //{
                    //    Console.SetCursorPosition(_e.ProgressPercentage + 1, ct + 1);
                    //    Console.Write("▄");
                    //    Console.SetCursorPosition(nl, ct + 1);
                    //    Console.Write("    ");
                    //    Console.SetCursorPosition(nl, ct + 1);
                    //    Console.Write("%" + _e.ProgressPercentage + "\n");
                    //    Console.Write(_e.TotalBytesToReceive.ToString());
                    //};
                    if (type == "file")
                    {
                        if (dltype == "async-task")
                        {
                            webClient.DownloadFileTaskAsync(source, output);
                        }
                        else if(dltype == "async")
                        {
                            webClient.DownloadFileAsync(new Uri(source), output);
                        }
                        else
                        {
                            webClient.DownloadFile(source, output);
                        }
                    }
                    else if (type == "string" || type == "text")
                    {
                        if (dltype == "async-task")
                        {
                            var str = webClient.DownloadStringTaskAsync(source).GetAwaiter().GetResult();
                            File.WriteAllText(output, str);
                        }
                        else if (dltype == "async")
                        {
                            /*var str = */webClient.DownloadStringAsync(new Uri(source));
                            //File.WriteAllText(output, str);
                        }
                        else
                        {
                            var str = webClient.DownloadString(source);
                            File.WriteAllText(output, str);
                        }
                    }
                };
                //bg.ProgressChanged += (s, e) =>
                //{
                //    //if(e.ProgressPercentage % 10 == 0)
                //    //{
                //    Console.SetCursorPosition(e.ProgressPercentage + 1, ct + 1);
                //    Console.Write("▄");
                //    //ll = Console.CursorLeft;
                //    //}
                //    Console.SetCursorPosition(nl, ct + 1);
                //    Console.Write("    ");
                //    Console.SetCursorPosition(nl, ct + 1);
                //    Console.Write("%" + e.ProgressPercentage + "\n");
                //};
                bg.RunWorkerCompleted += (s, e) =>
                {
                    if (!e.Cancelled)
                    {
                        Console.SetCursorPosition(0, ct + 1);
                        Console.Write("[");
                        for (int i = 0; i < 100; i++) Console.Write("▄");
                        Console.Write("] %100  \n");
                        Console.Write("    Download Completed\n\n");
                    }
                    else
                    {
                        Console.SetCursorPosition(4, ct + 2);
                        Console.Write("Download Canceled!\n\n");
                    }
                    ended = true;
                };
                bg.RunWorkerAsync();
                while (!ended) ;
                //Utils.PrintPrefix();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public override void Setup()
        {
            Name = "download";
            Aliases.Add("wget");
            Aliases.Add("dl");
            Usage = "download <-s|--source> [-o|--output] [-t|--type ('file'|'string')] [-dt|--dltype ('async'|'async-task'|'none')";
            Description = "Download a url";
        }
    }
}
