using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static void AddRegistry(ConsoleCommands commands)
        {
            if (ConsoleRegistries.ContainsKey(commands.CommandPrefix)) return;
            ConsoleRegistries.Add(commands.CommandPrefix, commands);
        }

        public static void RemoveRegistry(string prefix)
        {
            if (ConsoleRegistries.ContainsKey(prefix)) return;
            ConsoleRegistries.Remove(prefix);
        }

        public static List<(bool, string)> RunCommand(string inputString)
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