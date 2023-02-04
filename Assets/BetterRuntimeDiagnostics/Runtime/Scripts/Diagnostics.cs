using System.Runtime.CompilerServices;
using Better.Diagnostics.Runtime.DrawingModule;
using Better.Diagnostics.Runtime.DrawingModule.Framing;
using Better.Diagnostics.Runtime.DrawingModule.TrackableData;
using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public static class Diagnostics
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRay(Ray ray, Color color)
        {
            var wrapper =
                RemovablePool.Instance.GetWrapper<GenericLineWrapper, SingleUseFloatData, float, Vector3, Vector3, Color, float>(ray.origin, ray.direction,
                    color, 1f);
            FrameListener.AddWrapper(wrapper);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRay(Ray ray)
        {
            DrawRay(ray, Color.red);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            var vector3 = end - start;
            var wrapper = RemovablePool.Instance.GetWrapper<GenericLineWrapper, SingleUseFloatData, float, Vector3, Vector3, Color, float>(start,
                vector3.normalized, color, vector3.magnitude);
            FrameListener.AddWrapper(wrapper);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawLine(Vector3 start, Vector3 end)
        {
            DrawLine(start, end, Color.red);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRaycastHit(RaycastHit[] raycastHit)
        {
            DrawRaycastHit(raycastHit, Color.red);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRaycastHit(RaycastHit[] raycastHit, Color color)
        {
            foreach (var hit in raycastHit)
            {
                DrawRaycastHit(hit, color);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRaycastHit(RaycastHit hit)
        {
            DrawRaycastHit(hit, Color.red);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRaycastHit(RaycastHit hit, Color color)
        {
            var wrapper =
                RemovablePool.Instance.GetWrapper<GenericLineWrapper, SingleUseFloatData, float, Vector3, Vector3, Color, float>(hit.point, hit.normal, color,
                    0.3f);
            FrameListener.AddWrapper(wrapper);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawBoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float distance, Color color)
        {
            var end = center + direction * distance;

            var wrapper =
                RemovablePool.Instance.GetWrapper<BoxWrapper, SingleUseVector3Data, Vector3, Vector3, Quaternion, Color, Vector3>(end, orientation, color,
                    halfExtents * 2f);
            FrameListener.AddWrapper(wrapper);
            DrawLine(center, end, color);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawBoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float distance)
        {
            DrawBoxCast(center, halfExtents, direction, orientation, distance, Color.red);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawSphereCast(Vector3 center, float size, Vector3 direction, Quaternion orientation, float distance, Color color)
        {
            var end = center + direction * distance;

            var wrapper =
                RemovablePool.Instance.GetWrapper<GenericLineWrapper, SingleUseFloatData, float, Vector3, Quaternion, Color, float>(end, orientation, color,
                    size);
            FrameListener.AddWrapper(wrapper);
            DrawLine(center, end, color);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawSphereCast(Vector3 center, float size, Vector3 direction, Quaternion orientation, float distance)
        {
            var color = Color.red;
            DrawSphereCast(center, size, direction, orientation, distance, color);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRaycast(Ray ray, float lenght)
        {
            DrawLine(ray.origin, ray.origin + ray.direction * lenght);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRaycast(Ray ray, float lenght, Color color)
        {
            DrawLine(ray.origin, ray.origin + ray.direction * lenght, color);
        }
    }
}