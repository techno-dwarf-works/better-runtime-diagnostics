using System;
using System.Linq;
using System.Reflection;
using Better.Extensions.Runtime;

namespace Better.Diagnostics.EditorAddons.NodeEditor
{
    public  static class ReflectionExtensions
    {
        public static bool IsGenericAssignableFrom(this Type baseType, FieldInfo fieldInfo)
        {
            var interfaceType = fieldInfo.FieldType;
            if (baseType == null) return false;
            return interfaceType.GenericTypeArguments.Length > 0
                ? interfaceType.IsAssignableFrom(baseType)
                : baseType.GetInterfaces().Any(c => c.Name.FastEquals(fieldInfo.FieldType.Name));
        }

        public static bool IsAssignableFrom(this Type baseType, FieldInfo fieldInfo)
        {
            return baseType.IsAssignableFrom(fieldInfo.FieldType);
        }
    }
}