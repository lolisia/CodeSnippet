#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace aisilol
{
	public interface IGizmoDrawItem
	{
		Color Color { get; }
		void Draw(GameObject _target);
	}

	public class GizmoDrawHelper : Singleton<GizmoDrawHelper>
	{
		public void Add(GameObject _target, params IGizmoDrawItem[] items)
		{
			var list = mGizmoDrawItemListDic.Get(_target);
			list.AddRange(items);
		}
		public void Remove(GameObject _target)
		{
			mGizmoDrawItemListDic.Remove(_target);
		}

		void OnDrawGizmos()
		{
			mGizmoDrawItemListDic.ForEach((_target, _itemList) =>
			{
				_itemList.ForEach(_ => _.Draw(_target));
			});
		}

		private Dictionary<GameObject, List<IGizmoDrawItem>> mGizmoDrawItemListDic = new Dictionary<GameObject, List<IGizmoDrawItem>>();
	}


	public class GizmoDrawItem_DrawWireMesh : IGizmoDrawItem
	{
		public Color Color { get; private set; }
		public void Draw(GameObject _target)
		{
			if (mDrawMeshCache == null)
			{
				var filter = _target.GetComponent<MeshFilter>();
				if (filter == null)
					return;

				mDrawMeshCache = filter.sharedMesh;
			}

			using (new Gizmos_.ColorScope(Color))
			{
				if (mDrawMeshCache == null)
					return;

				var t = _target.transform;
				Gizmos.DrawWireMesh(mDrawMeshCache, t.position, t.rotation, t.lossyScale);
			}
		}

		public GizmoDrawItem_DrawWireMesh(Color _color) { Color = _color; }


		private Mesh mDrawMeshCache;
	}
	public class GizmoDrawItem_DrawText : IGizmoDrawItem
	{
		public Color Color { get; private set; }
		public void Draw(GameObject _target)
		{
			if (_target == null || mTextAction == null)
				return;

			if (mStyle == null)
			{
				mStyle = new GUIStyle("label")
				{
					alignment = TextAnchor.MiddleCenter
				};
				mStyle.normal.textColor = Color;
			}

			var pos = _target.transform.position + mDrawOffset;
			Handles.Label(pos, mTextAction(_target), mStyle);
		}

		public GizmoDrawItem_DrawText(Color _color, Func<GameObject, string> _textAction)
		{
			Color = _color;
			mTextAction = _textAction;
			mDrawOffset = Vector3.zero;
		}
		public GizmoDrawItem_DrawText(Color _color, Func<GameObject, string> _textAction, Vector3 _offset)
		{
			Color = _color;
			mTextAction = _textAction;
			mDrawOffset = _offset;
		}


		private Vector3 mDrawOffset;
		private GUIStyle mStyle;
		private Func<GameObject, string> mTextAction;
	}
}
#endif