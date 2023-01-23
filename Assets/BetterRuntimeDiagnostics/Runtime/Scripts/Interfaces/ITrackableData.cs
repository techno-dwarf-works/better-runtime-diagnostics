using UnityEngine;

namespace Better.Diagnostics.Runtime.Interfaces
{
    public interface ITrackableData<T>
    {
        public T Size { get; }
        public float OptionalSize { get; }
        Matrix4x4 Matrix4X4 { get; }
    }
}