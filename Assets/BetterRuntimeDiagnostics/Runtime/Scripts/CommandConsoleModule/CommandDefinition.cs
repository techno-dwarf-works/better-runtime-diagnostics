using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Better.Diagnostics.Runtime.CommandConsoleModule
{
    public static class CommandDefinition
    {
        public const string DefaultCommandPrefix = "better";
        internal const string CommandInputPrefix = "-";
        internal const string DoubleCommandInputPrefix = "--";
        internal const string PipeCommand = "|>";
        internal const char TypeSplitCommand = ':';
        internal const string HelpCommand = DoubleCommandInputPrefix + "help";

        private static readonly Dictionary<string, Type> TypeMap;

        static CommandDefinition()
        {
            TypeMap = new Dictionary<string, Type>();
            TypeMap.Add("int", typeof(int));
            TypeMap.Add("uint", typeof(uint));
            TypeMap.Add("long", typeof(long));
            TypeMap.Add("ulong", typeof(ulong));
            TypeMap.Add("short", typeof(short));
            TypeMap.Add("ushort", typeof(ushort));
            TypeMap.Add("float", typeof(float));
            TypeMap.Add("double", typeof(double));
            TypeMap.Add("decimal", typeof(decimal));
            TypeMap.Add("bool", typeof(bool));
            TypeMap.Add("char", typeof(char));
            TypeMap.Add("byte", typeof(byte));
            TypeMap.Add("date", typeof(DateTime));
        }

        //TODO add construct IConvertable
        public static void AddType<T>(string stringType) where T : IConvertible
        {
            if (TypeMap.ContainsKey(stringType))
                TypeMap.Add(stringType, typeof(T));
        }

        internal static bool TryParse(string stringValue, out object value, out Type objectType)
        {
            value = null;
            objectType = null;
            foreach (var type in TypeMap.Values)
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (!converter.IsValid(stringValue)) continue;
                value = converter.ConvertFromInvariantString(stringValue);
                objectType = type;
                return true;
            }

            return false;
        }

        internal static bool TryGetType(string stringType, out Type type)
        {
            return TypeMap.TryGetValue(stringType, out type);
        }
    }
}