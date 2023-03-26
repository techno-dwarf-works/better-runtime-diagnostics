using System;
using System.Collections.Generic;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;

namespace Better.Diagnostics.Runtime.DrawingModule
{
    internal class RemovablePool
    {
        private Dictionary<Type, List<IRemovable>> _dictionary;

        private static RemovablePool _instance;

        private RemovablePool()
        {
            _dictionary = new Dictionary<Type, List<IRemovable>>();
        }

        internal static RemovablePool Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RemovablePool();
                }

                return _instance;
            }
        }

        public void Add<T>(T item) where T : class, IRemovable, new()
        {
            var type = typeof(T);
            if (!_dictionary.TryGetValue(type, out var value))
            {
                value = new List<IRemovable>();
                _dictionary.Add(type, value);
            }

            value.Add(item);
        }

        public T Get<T>() where T : class, IRemovable, new()
        {
            var type = typeof(T);
            if (_dictionary.TryGetValue(type, out var value))
            {
                if (value.Count > 0)
                {
                    var removable = (T)value[0];
                    value.RemoveAt(0);
                    return removable;
                }
            }

            return new T();
        }

        public TType Get<TType, TData1, TData2, TData3>(TData1 data1, TData2 data2, TData3 data3)
            where TType : class, IRemovable, ISettable<TData1, TData2, TData3, TType>, new()
        {
            return Get<TType>().Set(data1, data2, data3);
        }

        public TRenderer Get<TRenderer, TWrapper, TData>(TData data)
            where TRenderer : class, IRemovable, ISettable<IRendererWrapper, IDiagnosticsRenderer>, new()
            where TWrapper : class, IRemovable, ISettable<TData, IRendererWrapper>, new()
        {
            var settable = (TRenderer)Get<TRenderer>().Set(Get<TWrapper>().Set(data));
            return settable;
        }

        public TWrapper GetWrapper<TWrapper, TTrackableData, TData>(TData data)
            where TWrapper : class, IRemovable, ISettable<TData, ITrackableData<TTrackableData>>, new()
        {
            var settable = (TWrapper)Get<TWrapper>().Set(data);
            return settable;
        }

        public TWrapper GetWrapper<TWrapper, TTrackable, TTrackableData, TData1, TData2, TData3, TData4>(TData1 data1, TData2 data2, TData3 data3, TData4 data4)
            where TWrapper : class, IRemovable, ISettable<ITrackableData<TTrackableData>, IRendererWrapper>, new()
            where TTrackable : class, ISettable<TData1, TData2, TData3, TData4, ITrackableData<TTrackableData>>, IRemovable, new()
        {
            var settable = (TWrapper)Get<TWrapper>().Set(Get<TTrackable>().Set(data1, data2, data3, data4));
            return settable;
        }

        public TRenderer Get<TRenderer, TWrapper, TTrackable, TTrackableData, TData>(TData data)
            where TRenderer : class, IRemovable, ISettable<IRendererWrapper, IDiagnosticsRenderer>, new()
            where TWrapper : class, IRemovable, ISettable<ITrackableData<TTrackableData>, IRendererWrapper>, new()
            where TTrackable : class, ITrackableData<TTrackableData>, ISettable<TData, ITrackableData<TTrackableData>>, new()
        {
            var settable = (TRenderer)Get<TRenderer>().Set(Get<TWrapper>().Set(Get<TTrackable>().Set(data)));
            return settable;
        }
    }
}