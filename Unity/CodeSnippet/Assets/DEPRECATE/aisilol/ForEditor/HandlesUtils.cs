#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace aisilol_Deprecate
{
	namespace Handles_
	{
		public class GUIScope : Scope
		{
			public GUIScope() { Handles.BeginGUI(); }
			protected override void CloseScope() { Handles.EndGUI(); }
		}

		public class ColorScope : Scope
		{
			public ColorScope(Color _color)
			{
				mOriginalColor = Handles.color;
				Handles.color = _color;
			}
			protected override void CloseScope() { Handles.color = mOriginalColor; }

			private Color mOriginalColor;
		}
	}
}
#endif