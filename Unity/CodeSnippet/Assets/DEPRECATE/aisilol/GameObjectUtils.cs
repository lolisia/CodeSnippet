using UnityEngine;

namespace aisilol_Deprecate
{
	public static class GameObject_
	{
		public static GameObject Create(string name, Transform parent = null)
		{
			var result = new GameObject(name);
			var t = result.transform;

			t.parent = parent;
			resetSRT(t);

			return result;
		}
		public static GameObject Instantiate(GameObject original, Transform parent = null)
		{
			var result = Object.Instantiate(original, parent);
			var t = result.transform;

			t.parent = parent;
			resetSRT(t);

			return result;
		}

		private static void resetSRT(Transform transform)
		{
			transform.localScale = Vector3.one;
			transform.localRotation = Quaternion.identity;
			transform.localPosition = Vector3.zero;
		}
	}
}