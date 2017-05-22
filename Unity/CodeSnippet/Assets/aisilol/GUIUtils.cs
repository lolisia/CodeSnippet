using UnityEngine;

namespace aisilol
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

			private Color mOriginalColor;
		}
	}
}