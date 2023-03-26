using System;
using System.Reflection;

namespace Better.Diagnostics.Runtime.CommandConsoleModule
{
    public class MethodData
    {
        private readonly MethodInfo _methodInfo;
        private readonly object _reference;
        private readonly bool _isInitiallyNull;

        public MethodData(MethodInfo methodInfo, object reference)
        {
            _methodInfo = methodInfo;
            _reference = reference;
            _isInitiallyNull = ReferenceEquals(_reference, null);
        }

        public ParameterInfo[] GetParameters()
        {
            return _methodInfo.GetParameters();
        }

        public object Invoke(object[] parameters)
        {
            if (!_isInitiallyNull)
            {
                if (ReferenceEquals(_reference, null))
                    throw new NullReferenceException("");
            }

            return _methodInfo.Invoke(_reference, parameters);
        }

        public bool Equal(MethodData methodData)
        {
            return _reference.Equals(methodData._reference);
        }
    }
}