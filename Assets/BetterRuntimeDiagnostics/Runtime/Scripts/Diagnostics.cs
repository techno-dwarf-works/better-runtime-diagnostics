using System.Runtime.CompilerServices;
using Better.Diagnostics.Runtime.Framing;
using Better.Diagnostics.Runtime.Models;
using Better.Diagnostics.Runtime.Models.Datas;
using UnityEngine;

namespace Better.Diagnostics.Runtime
{
    public static class Diagnostics
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawRay(Ray ray, Color color)
        {
            FrameListener.AddWrapper(new GenericLineWrapper(new SingleUseFloatData(ray.origin, ray.direction, 1f, color)));
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
            FrameListener.AddWrapper(new GenericLineWrapper(new SingleUseFloatData(start, vector3.normalized, vector3.magnitude, color)));
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
            FrameListener.AddWrapper(new SphereWrapper(new SingleUseFloatData(hit.point, hit.normal, 0.3f, color)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawBoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float distance, Color color)
        {
            var end = center + direction * distance;
            FrameListener.AddWrapper(new BoxWrapper(new SingleUseVector3Data(end, orientation, halfExtents * 2f, color)));
            DrawLine(center, end, color);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawBoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation, float distance)
        {
            DrawBoxCast(center, halfExtents, direction,orientation,distance ,Color.red);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawSphereCast(Vector3 center, float size, Vector3 direction, Quaternion orientation, float distance, Color color)
        {
            var end = center + direction * distance;
            FrameListener.AddWrapper(new SphereWrapper(new SingleUseFloatData(end, orientation, size, color)));
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