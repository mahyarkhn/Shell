using System.Collections.Generic;
using System.Linq;

namespace Shell.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool TryGetArgument(this IEnumerable<KeyValuePair<string, string>> keyValuePair, string key, out string value)
        {
            foreach(var kvp in keyValuePair)
            {
                if (key.Contains('|'))
                {
                    foreach(var spl in key.Split('|'))
                    {
                        if (kvp.Key == spl)
                        {
                            value = kvp.Value;
                            return true;
                        }
                    }
                }
                if(kvp.Key == key)
                {
                    value = kvp.Value;
                    return true;
                }
            }
            value = null;
            return false;
        }
        public static IEnumerable<KeyValuePair<string, string>> ArgumentsToDict(this string[] args)
        {
            string key = "";
            string value = "";

            foreach (var arg in args)
            {
                if (string.IsNullOrWhiteSpace(arg))
                    continue;

                if (arg.StartsWith("-"))
                {
                    key = arg.Split('=')[0];
                    if (arg.Contains("="))
                    {
                        value = string.Join("=", arg.Split('=').Skip(1));
                    }
                }
                else
                {
                    key = arg;
                    value = "";
                }

                yield return new KeyValuePair<string, string>(key, value);
                key = "";
                value = "";
            }
        }

        public static IEnumerable<string> ShellCommandLineToArray(this string str)
        {
            string tmp = "";
            int c = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '"')
                {
                    for (; i < str.Length; i++)
                    {
                        if (str[i] != '"')
                        {
                            tmp += str[i];
                        }
                        else
                        {
                            //i = j;
                            break;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(tmp))
                        yield return tmp;
                }
                else if (str[i] == '-')
                {
                    bool startString = false;
                    for (; i < str.Length; i++)
                    {
                        if (str[i] == '"')
                        {
                            i++;
                            if (startString)
                            {
                                startString = false;
                                break;
                            }
                            else
                            {
                                startString = true;
                            }
                        }
                        if (str[i] != ' ' || startString)
                        {
                            tmp += str[i];
                        }
                        else
                        {
                            //i = j;
                            break;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(tmp))
                        yield return tmp;
                }
                else if (str[i] != ' ')
                {
                    for (; i < str.Length; i++)
                    {
                        if (str[i] != ' ')
                        {
                            tmp += str[i];
                        }
                        else
                        {
                            //i = j;
                            break;
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(tmp))
                        yield return tmp;
                }
                tmp = "";
            }
        }
    }
}