using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models.Datas
{
    public class SingleUseFloatData : SingleUseData<float>
    {
        public SingleUseFloatData(Vector3 position, Vector3 normal, float size, Color color) : base(position, normal, color, size)
        {
        }
        public SingleUseFloatData(Vector3 position, Quaternion rotation, float size, Color color) : base(position, rotation, color, size)
        {
        }
    }

    public class SingleUseVector3Data : SingleUseData<Vector3>
    {
        public SingleUseVector3Data(Vector3 position, Vector3 normal, Vector3 size, Color color) : base(position, normal, color, size)
        {
        }
        public SingleUseVector3Data(Vector3 position, Quaternion rotation, Vector3 size, Color color) : base(position, rotation, color, size)
        {
        }
    }

    public abstract class SingleUseData<T> : ITrackableData<T>
    {
        private readonly Color _color;
        private readonly T _size;
        private bool _isMarked;

        public SingleUseData(Vector3 position, Quaternion rotation, Color color, T size)
        {
            Matrix4X4 = Matrix4x4.TRS(position, rotation, Vector3.one);
            _color = color;
            _size = size;
        }
        
        public SingleUseData(Vector3 position, Vector3 normal, Color color, T size)
        {
            Matrix4X4 = Matrix4x4.TRS(position, Quaternion.LookRotation(normal), Vector3.one);
            _color = color;
            _size = size;
        }

        public T Size => _size;
        public float OptionalSize { get; }
        public Color Color => _color;

        public bool IsMarkedForRemove
        {
            get
            {
                var isMarked = _isMarked;
                if (!isMarked)
                {
                    MarkForRemove();
                }

                return isMarked;
            }
        }

        public Matrix4x4 Matrix4X4 { get; }

        public void MarkForRemove()
        {
            _isMarked = true;
        }
    }
}