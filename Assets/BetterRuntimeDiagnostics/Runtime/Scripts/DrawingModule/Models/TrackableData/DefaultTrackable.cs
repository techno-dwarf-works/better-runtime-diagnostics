using System;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.DrawingModule.TrackableData
{
    [Serializable]
    public class DefaultTrackable : ITrackableData<float>
    {
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 scale = Vector3.one;
        [SerializeField] private Vector3 forward = Vector3.forward;
        [SerializeField] private Vector3 up = Vector3.up;
        [SerializeField] private float radius = 1;
        [SerializeField] private float height = 1;
        [SerializeField] private bool isNeedRemove;

        public float Size => radius;
        public float OptionalSize => height;
        public Matrix4x4 Matrix4X4 => Matrix4x4.TRS(position, Quaternion.LookRotation(forward, up), scale);
        public Color Color { get; } = Color.red;
        public bool IsMarkedForRemove => isNeedRemove;

        public void MarkForRemove()
        {
            isNeedRemove = true;
        }
        
        public void OnRemoved()
        {
            RemovablePool.Instance.Add(this);
        }
    }
}