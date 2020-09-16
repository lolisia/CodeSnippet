using UnityEngine;

namespace aisilol_Deprecate
{
	public static class GameObjectExtension
	{
		public static T AddComponentOnce<T>(this GameObject _object) where T : Component
		{
			var attachedComponent = _object.GetComponent<T>();
			return attachedComponent ? attachedComponent : _object.AddComponent<T>();
		}
	}
}
