using UnityEngine;

namespace aisilol
{
	public class GameObjectExtension
	{
		public static T AddComponentOnce<T>(GameObject _object) where T : Component
		{
			var attachedComponent = _object.GetComponent<T>();
			return attachedComponent ? attachedComponent : _object.AddComponent<T>();
		}
	}
}
