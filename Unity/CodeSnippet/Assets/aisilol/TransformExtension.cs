using UnityEngine;

public static class TransformExtension
{
	public static string GetPath(this Transform _transform)
	{
		var parent = _transform.parent;
		var result = _transform.name;

		while (parent != null)
		{
			result = parent.name + "/" + result;
			parent = parent.parent;
		}

		return result;
	}
}