using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Better.Diagnostics.Runtime.CommandConsoleModule
{
    public static class CommandDefinition
    {
        public const string DefaultCommandPrefix = "better";
        internal const char CommandInputPrefix = '-';
        internal const string DoubleCommandInputPrefix = "--";
        internal const string HelpCommand = DoubleCommandInputPrefix + "help";

        private static Dictionary<string, Type> _typeMap;

        static CommandDefinition()
        {
            _typeMap = new Dictionary<string, Type>();
            _typeMap.Add("int", typeof(int));
            _typeMap.Add("uint", typeof(uint));
            _typeMap.Add("long", typeof(long));
            _typeMap.Add("ulong", typeof(ulong));
            _typeMap.Add("short", typeof(short));
            _typeMap.Add("ushort", typeof(ushort));
            _typeMap.Add("float", typeof(float));
            _typeMap.Add("double", typeof(double));
            _typeMap.Add("decimal", typeof(decimal));
            _typeMap.Add("bool", typeof(bool));
            _typeMap.Add("char", typeof(char));
            _typeMap.Add("byte", typeof(byte));
            _typeMap.Add("date", typeof(DateTime));
        }

        //TODO add construct IConvertable
        public static void AddType(string stringType, Type type)
        {
            if (_typeMap.ContainsKey(stringType))
                _typeMap.Add(stringType, type);
        }

        public static bool TryParse(string stringValue, out object value, out Type objectType)
        {
            value = null;
            objectType = null;
            foreach (var type in _typeMap.Values)
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (!converter.IsValid(stringValue)) continue;
                value = converter.ConvertFromInvariantString(stringValue);
                objectType = type;
                return true;
            }

            return false;
        }

        public static bool TryGetType(string stringType, out Type type)
        {
            return _typeMap.TryGetValue(stringType, out type);
        }
    }
}