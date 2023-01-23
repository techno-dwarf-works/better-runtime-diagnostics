using System;
using Better.Diagnostics.Runtime.Interfaces;
using UnityEngine;

namespace Better.Diagnostics.Runtime.Models.Datas
{
    [Serializable]
    public class DefaultTrackable : ITrackableData<float>
    {
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 scale;
        [SerializeField] private Vector3 forward;
        [SerializeField] private float radius;
        [SerializeField] private float height;

        public float Size => radius;
        public float OptionalSize => height;
        public Matrix4x4 Matrix4X4 => Matrix4x4.TRS(position, Quaternion.LookRotation(forward), scale);
    }
}