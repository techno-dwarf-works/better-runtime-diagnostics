using System;
using UnityEngine;

namespace Better.Diagnostics.Runtime.CommandConsoleModule
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ConsoleCommandAttribute : Attribute
    {
        public bool IsValid { get; } = false;

        public string Command { get; }
        public string Prefix { get; }

        public ConsoleCommandAttribute(string prefix, string command)
        {
            if (command.Contains(' '))
            {
                Debug.Log($"Command does not support space character");
                return;
            }

            Prefix = prefix;
            IsValid = true;
            Command = command;
        }

        internal ConsoleCommandAttribute(string command) : this(CommandDefinition.DefaultCommandPrefix, command)
        {
        }
    }
}