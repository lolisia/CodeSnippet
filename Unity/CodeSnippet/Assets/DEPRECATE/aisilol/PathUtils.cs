using System;
using UnityEngine;

namespace aisilol_Deprecate
{
	public static class Path_
	{
		public static string AssetToAbsolute(string _assetPath)
		{
			var relativePath = _assetPath.Remove(0, "Assets/".Length);
			return CombinePath(Application.dataPath, relativePath);
		}
		public static string AbsoluteToAsset(string _path)
		{
			var absolutePath = ArrangePath(_path);

			if (!absolutePath.Contains(Application.dataPath))
				throw new ArgumentException(_path + " is not in asset path.");

			absolutePath = absolutePath.Replace(Application.dataPath, "");
			return CombinePath("Assets", absolutePath);
		}

		public static string ArrangePath(string _path)
		{
			return _path.Replace("\\", "/").Replace("//", "/");
		}
		public static string CombinePath(string _path1, string _path2)
		{
			return ArrangePath(ArrangePath(_path1) + "/" + ArrangePath(_path2));
		}
	}
}