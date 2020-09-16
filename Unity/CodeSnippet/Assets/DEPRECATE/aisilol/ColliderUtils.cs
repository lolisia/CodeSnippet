using UnityEngine;

namespace aisilol_Deprecate
{
	public static class Collider_
	{
		public static Bounds GetBounds(Transform _target)
		{
			var result = new Bounds(_target.position, Vector3.zero);

			foreach (var collider in _target.gameObject.GetComponentsInChildren<Collider>())
			{
				result.Encapsulate(collider.bounds);
			}

			return result;
		}
	}
}
