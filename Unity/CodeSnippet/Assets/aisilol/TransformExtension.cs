using aisilol;
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

	public static void SetWorldScale(this Transform _transform, Vector3 _lossyScale)
	{
		_transform.localScale = Vector3.one;
		var lossyScale = _transform.lossyScale;
		
		_transform.localScale = new Vector3(
			Math_.Divide(_lossyScale.x, lossyScale.x), 
			Math_.Divide(_lossyScale.y, lossyScale.y),
			Math_.Divide(_lossyScale.z, lossyScale.z));
	}

	public static void SetLocalTRS(this Transform _transform, Vector3 _position, Vector3 _euler, Vector3 _scale)
	{
		SetLocalTRS(_transform, _position, Quaternion.Euler(_euler), _scale);
	}
	public static void SetLocalTRS(this Transform _transform, Vector3 _position, Quaternion _rotation, Vector3 _scale)
	{
		_transform.localPosition = _position;
		_transform.localRotation = _rotation;
		_transform.localScale = _scale;
	}
}