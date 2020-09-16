using UnityEditor;
using UnityEngine;

namespace aisilol_Deprecate
{
	namespace GUI_
	{
		public class ColorScope : Scope
		{
			public ColorScope(Color _color)
			{
				mOriginalColor = GUI.color;
				GUI.color = _color;
			}
			protected override void CloseScope()
			{
				GUI.color = mOriginalColor;
			}

			private readonly Color mOriginalColor;
		}

		public class IndentScope : Scope
		{
			public IndentScope()
			{
				EditorGUI.indentLevel++;
			}
			protected override void CloseScope()
			{
				EditorGUI.indentLevel--;
			}
		}  
	}
}