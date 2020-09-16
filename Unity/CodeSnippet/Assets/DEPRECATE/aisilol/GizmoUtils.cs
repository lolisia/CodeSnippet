using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace aisilol_Deprecate
{
    public class GizmoMatrixScope : Scope
    {
        private readonly Matrix4x4 mSource;

        public GizmoMatrixScope(Matrix4x4 matrix)
        {
            mSource = Gizmos.matrix;
            Gizmos.matrix = matrix;
        }

        protected override void CloseScope()
        {
            Gizmos.matrix = mSource;
        }
    }

    public class GizmoColorScope : Scope
    {
        private readonly Color mSource;

        public GizmoColorScope(Color color)
        {
            mSource = color;
            Gizmos.color = color;
        }

        protected override void CloseScope()
        {
            Gizmos.color = mSource;
        }
    }

    public static class GizmoUtils
    {
        public static void DrawCircle(Vector3 _position, float _radius, Vector3 _up, Color _color)
        {
            using (new GizmoColorScope(_color))
            {
                var up = (_up == Vector3.zero ? Vector3.up : _up).normalized * _radius;
                var forward = Vector3.Slerp(up, -up, 0.5f); // up -> -up 사이의 중간
                var right = Vector3.Cross(up, forward).normalized * _radius;

                var matrix = new Matrix4x4();
                matrix[0] = right.x;
                matrix[1] = right.y;
                matrix[2] = right.z;

                matrix[4] = up.x;
                matrix[5] = up.y;
                matrix[6] = up.z;

                matrix[8] = forward.x;
                matrix[9] = forward.y;
                matrix[10] = forward.z;

                var current = _position + matrix.MultiplyPoint3x4(new Vector3(1, 0, 0));
                var next = Vector3.zero;

                for (var index = 0; index <= 90; index++)
                {
                    next.x = Mathf.Cos((index * 4) * Mathf.Deg2Rad);
                    next.z = Mathf.Sin((index * 4) * Mathf.Deg2Rad);
                    next.y = 0;

                    next = _position + matrix.MultiplyPoint3x4(next);

                    Gizmos.DrawLine(current, next);
                    current = next;
                }
            }
        }

        public static void DrawCapsule(Vector3 _start, Vector3 _end, float _radius, Color _color)
        {
            var up = (_end - _start).normalized * _radius;
            var forward = Vector3.Slerp(up, -up, 0.5f);
            var right = Vector3.Cross(up, forward).normalized * _radius;

            using (new GizmoColorScope(_color))
            {
                var height = (_start - _end).magnitude;
                var sideLength = Mathf.Max(0, height * 0.5f - _radius);
                var middle = (_start + _end) * 0.5f;

                var start = middle + (_start - middle).normalized * sideLength;
                var end = middle + (_end - middle).normalized * sideLength;

                DrawCircle(start, _radius, up, _color);
                DrawCircle(end, _radius, -up, _color);

                Gizmos.DrawLine(start + right, end + right);
                Gizmos.DrawLine(start - right, end - right);

                Gizmos.DrawLine(start + forward, end + forward);
                Gizmos.DrawLine(start - forward, end - forward);

                var count = 25f;

                for (var index = 1; index <= count; index++)
                {
                    var index1 = index;
                    
                    Action<Vector3, Vector3, Vector3> draw = (a, b, c) =>
                        Gizmos.DrawLine(Vector3.Slerp(a, b, index1 / count) + c, Vector3.Slerp(a, b, (index1 - 1) / count) + c);

                    draw(right, -up, start);
                    draw(-right, -up, start);
                    draw(forward, -up, start);
                    draw(-forward, -up, start);
                    
                    draw(right, up, end);
                    draw(-right, up, end);
                    draw(forward, up, end);
                    draw(-forward, up, end);
                }
            }
        }

        public static void DrawCollider(Collider _collider, Color _color)
        {
            using (new GizmoColorScope(_color))
            {
                if (_collider is BoxCollider)
                {
                    var box = _collider as BoxCollider;

                    var matrix = Matrix4x4.TRS(box.transform.TransformPoint(box.center), box.transform.rotation,
                        box.transform.lossyScale);

                    using (new GizmoMatrixScope(matrix))
                    {
                        Gizmos.DrawWireCube(Vector3.zero, box.size);
                    }

                    return;
                }

                if (_collider is SphereCollider)
                {
                    var sphere = _collider as SphereCollider;

                    var matrix = Matrix4x4.TRS(sphere.transform.TransformPoint(sphere.center),
                        sphere.transform.rotation,
                        sphere.transform.lossyScale);

                    using (new GizmoMatrixScope(matrix))
                    {
                        Gizmos.DrawWireSphere(Vector3.zero, sphere.radius);
                    }

                    DrawCircle(sphere.transform.position, sphere.radius * sphere.transform.lossyScale.x,
                        Camera.current.transform.forward * -1, _color);

                    return;
                }

                if (_collider is CapsuleCollider)
                {
                    var capsule = _collider as CapsuleCollider;

                    var lossyScale = capsule.transform.lossyScale;
                    var radiusScale = 0f;
                    var halfHeight = capsule.height * 0.5f;
                    var up = capsule.transform.up; 
                    
                    switch (capsule.direction)
                    {
                        case 0:
                            radiusScale = Mathf.Max(Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z));
                            halfHeight *= Mathf.Abs(lossyScale.x);
                            up = Quaternion.Euler(0, 0, 90) * up;
                            break;
                        case 1:
                            radiusScale = Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.z));
                            halfHeight *= Mathf.Abs(lossyScale.y);
                            break;
                        case 2:
                            radiusScale = Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y));
                            halfHeight *= Mathf.Abs(lossyScale.z);
                            up = Quaternion.Euler(90, 0, 0) * up;
                            break;
                    }

                    var radius = capsule.radius * radiusScale;
                    var start = capsule.bounds.center + up * halfHeight;
                    var end = capsule.bounds.center - up * halfHeight;
                    
                    DrawCapsule(start, end, radius, _color);
                    return;
                }

                if (_collider is MeshCollider)
                {
                    var mesh = _collider as MeshCollider;
                    
                    Gizmos.DrawWireMesh(mesh.sharedMesh, mesh.transform.position, mesh.transform.rotation, mesh.transform.lossyScale);
                }
            }
        }
    }

    public static class DrawGizmoCameraSelector
    {
        private static readonly HashSet<Camera> mUseCameraSet = new HashSet<Camera>();

        public static void Add(Camera camera)
        {
            mUseCameraSet.Add(camera);
        }
        public static void Clear()
        {
            mUseCameraSet.Clear();
        }
        
        public static bool DrawCurrent()
        {
#if UNITY_EDITOR
            if (SceneView.currentDrawingSceneView != null && SceneView.currentDrawingSceneView.camera == Camera.current)
                return true;
#endif

            return mUseCameraSet.Contains(Camera.current);
        }
    }
}