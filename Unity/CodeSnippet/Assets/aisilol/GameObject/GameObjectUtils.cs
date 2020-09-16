using UnityEngine;

namespace aisilol
{
    public static class GameObject_
    {
        public static T AddComponentOnce<T>(this GameObject _object) where T : Component
        {
            var attachedComponent = _object.GetComponent<T>();
            return attachedComponent != null ? attachedComponent : _object.AddComponent<T>();
        }
    }
}
