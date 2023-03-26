using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Better.Diagnostics.Runtime.CommandConsoleModule
{
    public class ConsoleCommands
    {
        private readonly Dictionary<string, MethodData> _staticMethods;


        public ConsoleCommands(Dictionary<string, MethodData> methodDatas)
        {
            _staticMethods = methodDatas;
        }

        internal (bool, string) Run(string inputString)
        {
            inputString = inputString.Trim();
            var length = inputString.IndexOf(' ');
            length = length < 0 ? inputString.Length : length;
            var command = inputString.Substring(0, length);

            var parametersString = inputString.Remove(0, length);
            var parameters = parametersString.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return RunCommand(command, parameters);
        }

        private (bool, string) RunCommand(string command, string[] parameters)
        {
            if (_staticMethods.TryGetValue(command, out var methodInfo))
            {
                var commands = parameters.Select(x => new CommandParameter(x)).ToArray();

                var infos = methodInfo.GetParameters();
                if (CheckHelpCommand(command, commands, infos, out var valueTuple)) return valueTuple;

                if (infos.Length != commands.Length)
                {
                    return (false, $"\"{command}\" has {infos.Length} parameters");
                }

                if (CheckForParams(infos, commands, out var runCommand)) return runCommand;

                if (!commands.All(x => x.IsValid))
                {
                    return (false, $"Some parameter is not valid");
                }

                var result = methodInfo.Invoke(commands.Select(x => x.Value).ToArray());
                return result == null ? (true, null) : (true, result.ToString());
            }

            return (false, "Command not found");
        }

        private static bool CheckForParams(ParameterInfo[] infos, CommandParameter[] commands, out (bool, string) runCommand)
        {
            if (!infos.Select(x => x.ParameterType).SequenceEqual(commands.Select(x => x.Type)))
            {
                var str = new StringBuilder("Parameters of command is:");
                foreach (var info in infos)
                {
                    str.Append($" {info.ParameterType}");
                }

                str.Append(" but invoked with:");

                foreach (var info in commands)
                {
                    str.Append($" {info.Type}");
                }

                runCommand = (false, str.ToString());
                return true;
            }

            runCommand = (false, null);
            return false;
        }

        private static bool CheckHelpCommand(string command, CommandParameter[] commands, ParameterInfo[] infos, out (bool, string) valueTuple)
        {
            if (commands.Any())
            {
                var first = commands.First();
                if (first.IsValid && first.IsHelpCommand)
                {
                    var str = new StringBuilder($"{command} has parameters:");
                    foreach (var parameterInfo in infos)
                    {
                        str.Append($" {parameterInfo.ParameterType}:{parameterInfo.Name}");
                    }

                    {
                        valueTuple = (true, str.ToString());
                        return true;
                    }
                }
            }

            valueTuple = (false, null);
            return false;
        }

        public void JoinCommands(ConsoleCommands commands)
        {
            foreach (var staticMethod in commands._staticMethods)
            {
                _staticMethods.Add(staticMethod.Key, staticMethod.Value);
            }
        }

        public void SubtractCommands(ConsoleCommands commands)
        {
            foreach (var staticMethod in commands._staticMethods)
            {
                if (_staticMethods.TryGetValue(staticMethod.Key, out var command))
                {
                    if (command.Equal(staticMethod.Value))
                        _staticMethods.Remove(staticMethod.Key);
                }
            }
        }
    }
}