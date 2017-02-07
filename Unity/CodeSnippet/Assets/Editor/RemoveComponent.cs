using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace aisilol
{
	public class RemoveComponentForPrefab
	{
		public static void RemoveComponent<T>() where T : Component
		{
			// Remove Component Type
			var assetPaths = AssetDatabase.GetAllAssetPaths();
			for (var index = 0; index < assetPaths.Length; index++)
			{
				var assetPath = assetPaths[index];

				var title = string.Format("Remove All Components For Prefab ({0}/{1})", index, assetPaths.Length);
				EditorUtility.DisplayProgressBar(title, assetPath, (float)index / assetPaths.Length);

				if (!assetPath.ToLower().EndsWith("prefab"))
					continue;

				var asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
				if (asset == null)
					continue;

				var components = asset.GetComponentsInChildren<T>();
				if (components.Length == 0)
					continue;

				// Check Out
				Provider.Checkout(asset, CheckoutMode.Exact).Wait();

				foreach (var c in components)
				{
					GameObject.DestroyImmediate(c, true);
				}

				EditorUtility.SetDirty(asset);
			}

			AssetDatabase.SaveAssets();
			EditorUtility.ClearProgressBar();
		}
	}
}