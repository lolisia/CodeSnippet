using UnityEditor;
using UnityEngine;

namespace aisilol
{
	public class GUIColorScope : Scope
	{
		public GUIColorScope(Color _color)
		{
			mOriginalColor = GUI.color;
			GUI.color = _color;
		}
		protected override void CloseScope()
		{
			GUI.color = mOriginalColor;
		}

		private Color mOriginalColor;
	}
	public class HandlesGUIScope : Scope
	{
		public HandlesGUIScope() { Handles.BeginGUI(); }
		protected override void CloseScope() { Handles.EndGUI(); }
	}
}