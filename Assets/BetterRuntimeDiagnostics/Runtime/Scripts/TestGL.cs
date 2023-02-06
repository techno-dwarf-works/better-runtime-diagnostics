using System;
using System.Linq;
using Better.Diagnostics.Runtime.DrawingModule;
using Better.Diagnostics.Runtime.DrawingModule.Framing;
using Better.Diagnostics.Runtime.DrawingModule.Interfaces;
using Better.Diagnostics.Runtime.DrawingModule.TrackableData;
using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public class TestGL : MonoBehaviour
    {
        [SerializeField] private DefaultTrackable sphereConeWrapper;
        [SerializeField] private DefaultTrackable squareConeWrapper;

        private void Start()
        {
            var removablePool = RemovablePool.Instance;

            FrameListener.AddRangeRenderer(FindObjectsOfType<BoxCollider>()
                .Select(removablePool.Get<GenericRenderer, BoxWrapper, BoxColliderData, Vector3, BoxCollider>));
            FrameListener.AddRangeRenderer(FindObjectsOfType<SphereCollider>()
                .Select(removablePool.Get<GenericRenderer, SphereWrapper, SphereColliderData, float, SphereCollider>));
            FrameListener.AddRangeRenderer(FindObjectsOfType<CapsuleCollider>()
                .Select(removablePool.Get<GenericRenderer, CapsuleWrapper, CapsuleColliderData, float, CapsuleCollider>));
            //FrameListener.AddRangeRenderer(FindObjectsOfType<Transform>().Select(removablePool.Get<GenericRenderer, AxisWrapper, TransformData, float, Transform>));

            FrameListener.AddRenderer(removablePool.Get<GenericRenderer>()
                .Set(removablePool.Get<SphereConeWrapper>().Set(sphereConeWrapper)));
            FrameListener.AddRenderer(removablePool.Get<GenericRenderer>()
                .Set(removablePool.Get<SphereConeWrapper>().Set(squareConeWrapper)));

            var t = FindObjectOfType<BoxCollider>();

            FrameListener.AddRenderer(removablePool.Get<GenericRenderer, SphereConeWrapper, DefaultTrackable>(sphereConeWrapper));
            FrameListener.AddRenderer(removablePool.Get<GenericRenderer, SquareConeWrapper, DefaultTrackable>(squareConeWrapper));
        }

        private void Update()
        {
            var transform1 = transform;
            Diagnostic.DrawRay(new Ray(transform1.position, transform1.forward), Color.cyan);
        }
    }
}