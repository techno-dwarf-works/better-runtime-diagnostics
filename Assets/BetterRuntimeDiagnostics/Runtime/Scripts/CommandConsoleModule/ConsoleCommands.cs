using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Better.Extensions.Runtime;

namespace Better.Diagnostics.Runtime.CommandConsoleModule
{
    public class ConsoleCommands
    {
        private readonly Dictionary<string, MethodInfo> _staticMethods;

        private readonly string _commandPrefix;

        internal string CommandPrefix => _commandPrefix;

        public ConsoleCommands(string commandPrefix)
        {
            _commandPrefix = commandPrefix;
            _staticMethods = new Dictionary<string, MethodInfo>();
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().SelectMany(type =>
                    type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Where(info => info.IsStatic)));
            foreach (var methodInfo in types)
            {
                var command = methodInfo.GetCustomAttribute<ConsoleCommandAttribute>();
                if (command != null && command.IsValid && command.Prefix.FastEquals(commandPrefix))
                {
                    _staticMethods.Add(command.Command, methodInfo);
                }
            }
        }

        //TODO: Add pipe better -run 10 |> -run2 -homeRun true
        //TODO: -first |> -second == send return value from -first to second

        public (bool, string) Run(string inputString)
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

                        return (true, str.ToString());
                    }
                }

                if (infos.Length != commands.Length)
                {
                    return (false, $"\"{command}\" has {infos.Length} parameters");
                }

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
                    
                    return (false, str.ToString());
                }

                if (!commands.All(x => x.IsValid))
                {
                    return (false, $"Some command is not valid");
                }

                var result = methodInfo.Invoke(null, commands.Select(x => x.Value).ToArray());
                return result == null ? (true, null) : (true, result.ToString());
            }

            return (false, "Command not found");
        }
    }
}