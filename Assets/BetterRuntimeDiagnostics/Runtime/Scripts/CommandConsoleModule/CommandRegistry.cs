using System.Collections.Generic;

namespace Better.Diagnostics.Runtime.CommandConsoleModule
{
    public static class CommandRegistry
    {
        private static Dictionary<string, ConsoleCommands> _consoleRegistries;

        static CommandRegistry()
        {
            _consoleRegistries = new Dictionary<string, ConsoleCommands>();
        }

        public static void AddRegistry(ConsoleCommands commands)
        {
            if(_consoleRegistries.ContainsKey(commands.CommandPrefix)) return;
            _consoleRegistries.Add(commands.CommandPrefix, commands);
        }
        
        public static void RemoveRegistry(string prefix)
        {
            if(_consoleRegistries.ContainsKey(prefix)) return;
            _consoleRegistries.Remove(prefix);
        }

        public static (bool, string) RunCommand(string inputString)
        {
            inputString = inputString.Trim();
            var length = inputString.IndexOf(' ');
            length = length < 0 ? inputString.Length : length;
            var commandPrefix = inputString.Substring(0, length);
            if (_consoleRegistries.TryGetValue(commandPrefix, out var value))
            {
                var command = inputString.Remove(0, length);
                return value.Run(command);
            }

            return (false, $"No console command with prefix {commandPrefix}");
        }
    }
}