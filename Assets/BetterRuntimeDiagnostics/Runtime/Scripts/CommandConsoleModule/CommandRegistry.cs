using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Better.Extensions.Runtime;

namespace Better.Diagnostics.Runtime.CommandConsoleModule
{
    public static class CommandRegistry
    {
        private static readonly Dictionary<string, ConsoleCommands> ConsoleRegistries;
        private static readonly Regex CommandRegex;

        static CommandRegistry()
        {
            ConsoleRegistries = new Dictionary<string, ConsoleCommands>();
            CommandRegex = new Regex($@"(?<!{CommandDefinition.CommandInputPrefix}){CommandDefinition.CommandInputPrefix}\b+", RegexOptions.CultureInvariant);
        }

        public static void RegisterStatics(string prefix)
        {
            var staticMethods = new Dictionary<string, MethodData>();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().SelectMany(type =>
                    type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Where(info => info.IsStatic)));
            foreach (var methodInfo in types)
            {
                var command = methodInfo.GetCustomAttribute<ConsoleCommandAttribute>();
                if (command != null && command.IsValid && command.Prefix.FastEquals(prefix))
                {
                    staticMethods.Add(command.Command, new MethodData(methodInfo, null));
                }
            }

            var defaultConsoleCommands = new ConsoleCommands(staticMethods);
            AddRegistry(prefix, defaultConsoleCommands);
        }

        //TODO: Make it beautiful
        public static void RegisterInstance(object obj)
        {
            if(ReferenceEquals(obj, null)) return;
            var methodDatas = GatherCommandsFromInstance(obj);

            foreach (var methodData in methodDatas)
            {
                AddRegistry(methodData.Key, new ConsoleCommands(methodData.Value));
            }
        }
        
        //TODO: Make it beautiful
        public static void UnregisterInstance(object obj)
        {
            if(ReferenceEquals(obj, null)) return;
            var methodDatas = GatherCommandsFromInstance(obj);

            foreach (var methodData in methodDatas)
            {
                if (ConsoleRegistries.TryGetValue(methodData.Key, out var command))
                {
                    command.SubtractCommands(new ConsoleCommands(methodData.Value));
                }
            }
        }

        private static Dictionary<string, Dictionary<string, MethodData>> GatherCommandsFromInstance(object obj)
        {
            var methodInfos = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(x => !x.IsStatic);

            var methodDatas = new Dictionary<string, Dictionary<string, MethodData>>();
            foreach (var methodInfo in methodInfos)
            {
                var command = methodInfo.GetCustomAttribute<ConsoleCommandAttribute>();
                if (command == null || !command.IsValid) continue;

                if (!methodDatas.TryGetValue(command.Prefix, out var dictionary))
                {
                    dictionary = new Dictionary<string, MethodData>();
                    methodDatas.Add(command.Prefix, dictionary);
                }

                dictionary.Add(command.Command, new MethodData(methodInfo, obj));
            }

            return methodDatas;
        }

        public static void AddRegistry(string prefix, ConsoleCommands commands)
        {
            if (ConsoleRegistries.TryGetValue(prefix, out var command))
            {
                command.JoinCommands(commands);
            }
            else
            {
                ConsoleRegistries.Add(prefix, commands);
            }
        }

        public static void RemoveRegistry(string prefix)
        {
            if (ConsoleRegistries.ContainsKey(prefix)) return;
            ConsoleRegistries.Remove(prefix);
        }

        internal static List<(bool, string)> RunCommand(string inputString)
        {
            inputString = inputString.Trim();
            var length = inputString.IndexOf(' ');
            length = length < 0 ? inputString.Length : length;
            var commandPrefix = inputString.Substring(0, length);
            if (!ConsoleRegistries.TryGetValue(commandPrefix, out var value))
                return new List<(bool, string)> { (false, $"No console command with prefix {commandPrefix}") };

            var command = inputString.Remove(0, length).Trim();
            var items = CommandRegex.Split(command).Where(s => s != string.Empty).ToArray();
            var list = new List<(bool, string)>(items.Length);
            string response = null;
            foreach (var commandItem in items)
            {
                var (newString, isCommandValid) = PreprocessPipeCommand(commandItem, ref response);

                var result = value.Run(newString);
                list.Add(result);
                if (ValidateCommandResult(isCommandValid, result, ref response)) break;
            }

            return list;
        }

        private static (string, bool) PreprocessPipeCommand(string item, ref string response)
        {
            var endWithPipe = item.Trim().EndsWith(CommandDefinition.PipeCommand);
            if (endWithPipe)
            {
                item = item.Replace(CommandDefinition.PipeCommand, string.Empty).Trim();
            }

            if (!string.IsNullOrEmpty(response))
            {
                if (!item.EndsWith(' '))
                {
                    item += " ";
                }

                item += response;
                response = null;
            }

            return (item, endWithPipe);
        }

        private static bool ValidateCommandResult(bool endWithPipe, (bool, string) result, ref string response)
        {
            if (endWithPipe)
            {
                if (result.Item1)
                {
                    response = result.Item2;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}