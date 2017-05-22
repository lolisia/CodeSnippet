using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace aisilol
{
	public class ScopeProfilerWindow : EditorWindow
	{
		[MenuItem("Window/aisilol/Scope Profiler")]
		public static void ShowWindow()
		{
			var window = GetWindow<ScopeProfilerWindow>();
			window.titleContent = new GUIContent("Scope Profiler");
			window.Show();
		}

		public ScopeProfilerWindow()
		{
			mColumns.Add(new GUIContent("Profile"));
			mColumns.Add(new GUIContent("Call Count"));
			mColumns.Add(new GUIContent("Elapsed"));
			mColumns.Add(new GUIContent("Average"));
		}
		void Update()
		{
			if (Time.realtimeSinceStartup < mNextUpdate)
				return;

			mNextUpdate = Time.realtimeSinceStartup + 0.1f;
			Repaint();
		}
		void OnGUI()
		{
			using (var scroll = new GUILayout.ScrollViewScope(mScrollPosition))
			{
				mScrollPosition = scroll.scrollPosition;

				using (new ScopeProfiler("ScopeProfilerWindow " + Time.realtimeSinceStartup))
				{
					if (GUILayoutUtils.SizeButton(new GUIContent("Clear Data")))
					{
						ScopeProfilerManager.Clear();
					}

					mRows.Clear();

					foreach (var pair in ScopeProfilerManager.GetResult())
					{
						var row = new List<GUIContent>();
						row.Add(new GUIContent(pair.Key));
						row.Add(new GUIContent(pair.Value.CallCount.ToString()));
						row.Add(new GUIContent(pair.Value.Elapsed.ToString("N2")));
						row.Add(new GUIContent(pair.Value.Average.ToString("N2") + "/s"));

						mRows.Add(row);
					}

					mSelectedGridIndex = GUILayoutUtils.Grid(mSelectedGridIndex, mColumns, mRows);
				}
			}
		}

		private List<GUIContent> mColumns = new List<GUIContent>();
		private List<List<GUIContent>> mRows = new List<List<GUIContent>>();

		private int mSelectedGridIndex;
		private float mNextUpdate;
		private Vector2 mScrollPosition;
	}
}