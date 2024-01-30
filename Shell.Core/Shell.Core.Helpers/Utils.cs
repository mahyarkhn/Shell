using Shell.Core.Enums;
using Shell.Core.Internal;
using System;
using System.Linq;


namespace Shell.Core.Helpers
{
    public class Utils
    {
        public static void PrintPrefix()
        {
            SmartPrint("^15[^3#^15] ^3> ");
        }

        public static void Print()
        {

        }

        public static void Print(LogLevel logLevel, object value)
        {
            var str = value.ToString();
            var valueForeColor = 15;
            var prefixColor = 7;
            var prefix = "#";
            if (logLevel == LogLevel.Response)
            {
                prefix = "R";
                prefixColor = 2;
            }
            else if (logLevel == LogLevel.Request)
            {
                prefix = "?";
                prefixColor = 3;
            }
            else if (logLevel == LogLevel.Info)
            {
                prefix = "I";
                prefixColor = 10;
            }
            else if (logLevel == LogLevel.Error)
            {
                prefix = "E";
                prefixColor = 4;
            }
            else if (logLevel == LogLevel.Warning)
            {
                prefix = "W";
                prefixColor = 6;
            }
            else if (logLevel == LogLevel.Trace)
            {
                prefix = "T";
                prefixColor = 8;
                valueForeColor = 7;
            }
            else
            {
                prefix = "#";
                prefixColor = 8;
                valueForeColor = 7;
            }

            var currentForeColor = Console.ForegroundColor;
            SmartPrintLn($"^{valueForeColor}[^{prefixColor}{prefix}^{valueForeColor}]: {str}");
        }
        public static void PrintLn(LogLevel logLevel, object value)
        {
            Print(logLevel, value + "\n");
        }

        public static void PrintDebug(object value)
        {
            Print(LogLevel.Debug, value);
        }

        public static void PrintError(object value)
        {
            Print(LogLevel.Error, value);
        }

        public static void PrintInfo(object value)
        {
            Print(LogLevel.Info, value);
        }

        public static void PrintRequest(object value)
        {
            Print(LogLevel.Request, value);
        }

        public static void PrintResponse(object value)
        {
            Print(LogLevel.Response, value);
        }

        public static void PrintTrace(object value)
        {
            Print(LogLevel.Trace, value);
        }

        public static void PrintWarning(object value)
        {
            Print(LogLevel.Warning, value);
        }

        public static void SmartPrint(object value)
        {
            var currentForeColor = Console.ForegroundColor;
            var currentBackColor = Console.BackgroundColor;
            //var newForeColor = Console.ForegroundColor;
            var data = value.ToString();
            bool escape = false;
            for (int i = 0; i < data.Length; i++)
            {
                int skip = 0;
                switch (data.ElementAt(i))
                {
                    case Constants.SmartLog_EscapeSign: // escape codes
                        {
                            if (!Constants.SmartLog_EscapeCodes.Contains(data.ElementAt(i + 1)))
                            {
                                escape = true;
                            }
                        }
                        break;

                    case Constants.SmartLog_ForeColorSign: // foreground color
                        {
                            if (escape)
                            {
                                escape = false;
                            }
                            else if (int.TryParse(data.ElementAt(i + 1).ToString(), out int colorCode))
                            {
                                skip = 1;
                                if (int.TryParse(data.ElementAt(i + 2).ToString(), out int colorCode2))
                                {
                                    colorCode = int.Parse($"{colorCode}{colorCode2}");
                                    skip = 2;
                                }
                                if (Enum.GetNames(typeof(ConsoleColor)).Length > colorCode)
                                {
                                    if (Enum.TryParse<ConsoleColor>(Enum.GetNames(typeof(ConsoleColor))[colorCode], out var foreColor))
                                    {
                                        Console.ForegroundColor = foreColor;
                                        //newForeColor = foreColor;
                                        i += skip;
                                        continue;
                                    }
                                }
                            }
                        }
                        break;
                }
                if (!escape)
                {
                    Console.Write(data.ElementAt(i));
                }
            }
            Console.ForegroundColor = currentForeColor;
            Console.BackgroundColor = currentBackColor;
        }
        public static void SmartPrintLn(object value)
        {
            SmartPrint(value + "\n");
        }
    }
}