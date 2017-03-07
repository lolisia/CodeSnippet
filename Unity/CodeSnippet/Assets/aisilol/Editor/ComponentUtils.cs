using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace aisilol
{
	public static class ComponentUtils
	{
		public static List<GameObject> FindMissingComponentAssets()
		{
			// Prefab Preload
			var result = new List<GameObject>();
			foreach (var assetPath in AssetDatabase.FindAssets("t:GameObject"))
			{
				AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
			}

			foreach (var instance in Resources.FindObjectsOfTypeAll<GameObject>())
			{
				foreach (var component in instance.GetComponents<Component>())
				{
					if (component != null)
						continue;

					result.Add(instance);
				}
			}

			return result;
		}
	}
}