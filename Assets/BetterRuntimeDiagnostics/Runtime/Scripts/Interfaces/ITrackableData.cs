using UnityEngine;

namespace Better.Diagnostics.Runtime.Interfaces
{
    public interface ITrackableData<out T> : IRemovable
    {
        public T Size { get; }
        public float OptionalSize { get; }
        public Matrix4x4 Matrix4X4 { get; }
        public Color Color { get; }
    }
}