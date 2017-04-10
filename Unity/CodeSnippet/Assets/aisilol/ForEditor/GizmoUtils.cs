using UnityEngine;

namespace aisilol
{
	namespace Gizmos_
	{
		public class ColorScope : Scope
		{
			public ColorScope(Color _color)
			{
				mOriginalColor = Gizmos.color;
				Gizmos.color = _color;
			}
			protected override void CloseScope()
			{
				Gizmos.color = mOriginalColor;
			}

			private Color mOriginalColor;
		}
	}
}