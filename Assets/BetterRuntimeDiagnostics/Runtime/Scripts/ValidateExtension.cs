using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public static class ValidateExtension
    {
        public static bool IsNullOrDestroyed(this Object obj) {
 
            if (ReferenceEquals(obj, null)) return true;
 
            return obj == null;
        }
    }
}