using System;
using System.Linq;
using Better.Diagnostics.Runtime.DrawingModule;
using Better.Diagnostics.Runtime.DrawingModule.Framing;
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
            FrameListener.AddRangeRenderer(FindObjectsOfType<BoxCollider>().Select(x => new ColliderRenderer(x)));
            FrameListener.AddRangeRenderer(FindObjectsOfType<SphereCollider>().Select(x => new ColliderRenderer(x)));
            FrameListener.AddRangeRenderer(FindObjectsOfType<CapsuleCollider>().Select(x => new ColliderRenderer(x)));
            //GLDrawer.AddRangeRenderer(FindObjectsOfType<Transform>().Select(x => new GenericRenderer(new AxisWrapper(new TransformData(x)))));
            FrameListener.AddRenderer(new GenericRenderer(new SphereConeWrapper(sphereConeWrapper)));
            FrameListener.AddRenderer(new GenericRenderer(new SquareConeWrapper(squareConeWrapper)));
        }

        private void Update()
        {
            var transform1 = transform;
            Diagnostics.DrawRay(new Ray(transform1.position, transform1.forward), Color.cyan);
        }
    }
}