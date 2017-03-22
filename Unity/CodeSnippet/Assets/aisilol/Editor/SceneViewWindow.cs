using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace aisilol
{
	//   ____________         _________________________
	//  /           /|       /                        /|
	// +-----------+ | ---- +------------------------+ |     +-----------------+
	// | SceneView | |      | SceneViewWindowManager | | --- | SceneViewWindow |
	// +-----------+/       +------------------------+/      +-----------------+
	//

	// NOTE : SceneView별 좌표를 별도로 관리하고 Data는 공통으로 사용하기 위해, SceneViewWindowManager에서 좌표 및 SceneView별 Instance관리를 처리한다.
	// SceneViewWindow는 Contents Data및 UI코드를 관리하고, 단일 Instance를 사용한다.
	public abstract class SceneViewWindow
	{
		// NOTE : SceneViewWindow를 상속받은 class에서 아래 함수를 구현한다.
		//	[InitializeOnLoadMethod]
		//	public static void OnInitialize()
		//	{
		//		SceneView.onSceneGUIDelegate += SceneViewWindowManager<T>.OnSceneGUI;
		//	}

		public virtual bool UseInPlaying { get { return true; } }
		public virtual bool Draggable { get { return true; } }
		public abstract Vector2 GetDefaultPosition(Rect sceneViewRect);
		public abstract string Name { get; }
		public abstract void OnSceneGUI();

		public bool NeedUpdateWindowSize { get; set; }
		public void UpdateWindowSize()
		{
			NeedUpdateWindowSize = true;
		}
	}

	public class SceneViewWindowManager<T>
		where T : SceneViewWindow, new()
	{
		private static Dictionary<SceneView, SceneViewWindowManager<T>> mInstanceDic = new Dictionary<SceneView, SceneViewWindowManager<T>>();
		private static T mWindow = new T();

		private Rect mWindowRect;
		private bool mInitialized;
		private bool mNeedUpdateWindowSize;

		public static void OnSceneGUI(SceneView view)
		{
			var instance = mInstanceDic.Get(view);
			instance.DrawSceneGUI(view);
		}

		private void DrawSceneGUI(SceneView view)
		{
			if (Event.current.type != EventType.Layout)
				return;

			if (!mWindow.UseInPlaying)
			{
				if (Application.isPlaying)
					return;
			}

			if (!mInitialized)
			{
				mInitialized = true;

				// 초기 Window Position으로 Rect를 설정한다.
				mWindowRect = new Rect(mWindow.GetDefaultPosition(view.camera.pixelRect), Vector2.zero);
			}

			using (new Handles_.GUIScope())
			{
				if (mNeedUpdateWindowSize)
				{
					mWindowRect = new Rect(mWindowRect.position, Vector2.zero);
					mNeedUpdateWindowSize = false;
					view.Repaint();
				}

				var id = GUIUtility.GetControlID(FocusType.Passive, mWindowRect);
				mWindowRect = GUILayout.Window(id, mWindowRect, DrawWindow, mWindow.Name);

				// Window가 SceneView 영역에서 벗어날 경우, 초기 Window Position으로 이동시킨다.
				if (!view.camera.pixelRect.Contains(new Vector2(mWindowRect.x, mWindowRect.y)))
					mWindowRect = new Rect(mWindow.GetDefaultPosition(view.camera.pixelRect), Vector2.zero);

				// Window Size를 갱신해야 한다면, 모든 SceneViewWindow의 Update를 예약한다.
				if (mWindow.NeedUpdateWindowSize)
				{
					mWindow.NeedUpdateWindowSize = false;
					foreach (var pair in mInstanceDic)
					{
						pair.Value.mNeedUpdateWindowSize = true;
					}

					SceneView.RepaintAll();
				}
			}
		}
		private void DrawWindow(int id)
		{
			mWindow.OnSceneGUI();

			if (mWindow.Draggable)
				GUI.DragWindow();
		}
	}
}