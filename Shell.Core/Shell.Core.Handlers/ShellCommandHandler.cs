using Shell.Core.Abstracts;
using Shell.Core.Enums;
using Shell.Core.Exceptions;
using Shell.Core.Extensions;
using Shell.Core.Helpers;
using Shell.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shell.Core.Handlers
{
    public class ShellCommandHandler : IShellCommandHandler
    {
        public static IShellCommandHandler Instance = new ShellCommandHandler();

        private Dictionary<string, IShellCommand> m_shellcommands = new Dictionary<string, IShellCommand>();

        public IEnumerable<IShellCommand> this[string name]
        {
            get
            {
                var result = (from shellcommand in m_shellcommands
                              where (shellcommand.Key.ToLower().Contains(name.ToLower()) || shellcommand.Value.Aliases.Contains(name.ToLower()))
                              select shellcommand.Value);
                return result;
            }
        }

        public IEnumerable<IShellCommand> GetShellCommands()
        {
            return m_shellcommands.Values.AsEnumerable<IShellCommand>();
        }

        public ShellCommandHandler()
        {
            SetupAssembly(Assembly.GetExecutingAssembly());
        }

        public bool Handle(string name, params string[] args)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name)) return false;
                var foundshellcommands = this[name];

                if(foundshellcommands == null)
                {
                    throw new NullReferenceException();
                }
                if(foundshellcommands.Count() != 1)
                {
                    throw new ShellCommandNotFoundException();
                }
                if(foundshellcommands.First() == null)
                {
                    throw new NullReferenceException();
                }
                var targetshellcommand = foundshellcommands.First();

                if (args != null && args.Length > 0)
                {
                    if (args.ArgumentsToDict().TryGetArgument("-u|--usage", out var usage))
                    {
                        Utils.SmartPrintLn(targetshellcommand.ShellCommandToUsage());
                    }
                    else if (args.ArgumentsToDict().TryGetArgument("-h|--help|/?", out var help))
                    {
                        Utils.SmartPrintLn(targetshellcommand.ShellCommandToHelp());
                    }
                    else
                    {
                        targetshellcommand.Execute(args);
                    }
                }
                else
                {
                    targetshellcommand.Execute();
                }
                Console.WriteLine();

                return true;
            }
            catch (ShellCommandNotFoundException ex)
            {
                if(!ex.Data.Contains("name"))
                    ex.Data["name"] = name;
                ex.PrintShellCommandNotFoundException();
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SetupAssembly(Assembly assembly)
        {
            try
            {
                if (assembly == null)
                    throw new ArgumentNullException("assembly");

                var types = (from type in assembly.GetTypes() where (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ShellCommand))) select type);

                foreach (var type in types)
                {
                    if (type == null)
                        //throw new NullReferenceException();
                        continue;

                    SetupType(type);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SetupType(Type type)
        {
            try
            {
                if (type == null)
                    throw new ArgumentNullException("type");

                object obj = Activator.CreateInstance(type);
                var shellcommand = obj as IShellCommand;
                shellcommand.Setup();

#if !DEBUG
                if (shellcommand.ShellCommandType == ShellCommandType.Debug) return;
#endif

                if (m_shellcommands.ContainsKey(shellcommand.Name.ToLower()) || m_shellcommands.ContainsValue(shellcommand))
                    throw new ShellCommandAlreadyExistsException(shellcommand.Name.ToLower());

                m_shellcommands.Add(shellcommand.Name.ToLower(), shellcommand);
            }
            catch (ShellCommandAlreadyExistsException ex)
            {
                ex.Data["name"] = ex.Message;
                ex.PrintShellCommandAlreadyExistsException();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}