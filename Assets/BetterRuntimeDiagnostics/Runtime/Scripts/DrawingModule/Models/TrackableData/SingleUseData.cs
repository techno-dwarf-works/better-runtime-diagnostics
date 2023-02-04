using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule.TrackableData
{
    public abstract class SingleUseData<T> : ITrackableData<T>, ISettable<Vector3, Quaternion, Color, T, ITrackableData<T>>, ISettable<Vector3, Vector3, Color, T, ITrackableData<T>>
    {
        private Color _color;
        private T _size;
        private bool _isMarked;
        private Matrix4x4 _matrix4X4;

        public ITrackableData<T> Set(Vector3 position, Quaternion rotation, Color color, T size)
        {
            _isMarked = false;
            _matrix4X4 = Matrix4x4.TRS(position, rotation, Vector3.one);
            _color = color;
            _size = size;
            return this;
        }
        
        public ITrackableData<T> Set(Vector3 position, Vector3 normal, Color color, T size)
        {
            _isMarked = false;
            _matrix4X4 = Matrix4x4.TRS(position, Quaternion.LookRotation(normal), Vector3.one);
            _color = color;
            _size = size;
            return this;
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

        public Matrix4x4 Matrix4X4 => _matrix4X4;

        public void MarkForRemove()
        {
            _isMarked = true;
        }

        public abstract void OnRemoved();
    }
}