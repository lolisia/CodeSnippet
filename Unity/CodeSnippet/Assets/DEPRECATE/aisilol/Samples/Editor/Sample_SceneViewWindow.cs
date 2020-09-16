using UnityEditor;
using UnityEngine;

namespace aisilol_Deprecate
{
	public class Sample_SceneViewWindow : SceneViewWindow
	{
		public override string Name { get { return "Sample"; } }

		[InitializeOnLoadMethod]
		public static void OnInitialize()
		{
			SceneView.duringSceneGui += SceneViewWindowManager<Sample_SceneViewWindow>.OnSceneGUI;
		}

		public override Vector2 GetDefaultPosition(Rect sceneViewRect)
		{
			// SceneView의 (100, 100) 위치에 윈도우를 구성한다.
			return new Vector2(100, 100);
		}

		private int mClickCount;
		private bool mVisible = true;
		public override void OnSceneGUI()
		{
			using (new GUILayout.VerticalScope())
			{
				if (GUILayout.Button(mVisible ? "Hide" : "Show"))
				{
					mVisible = !mVisible;
					UpdateWindowSize();     // Window의 Size가 줄어들어야 할 경우, 해당 함수를 호출해줘야 한다.
				}

				if (mVisible)
				{
					if (GUILayout.Button("Click! " + mClickCount))
						mClickCount++;
				}
			}
		}
	}
}