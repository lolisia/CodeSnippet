using System.Collections;
using UnityEngine;

namespace aisilol
{
	public class CoroutineRunner : MonoBehaviour
	{
		public static void Run(IEnumerator _routine)
		{
			if (_routine == null)
				return;

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				new EditorCoroutineRunner(_routine);
				return;
			}
#endif
			if (sRunner == null)
			{
				var instance = new GameObject("[CoroutineRunner]");
				DontDestroyOnLoad(instance);

				sRunner = instance.AddComponent<CoroutineRunner>();
			}

			sRunner.StartCoroutine(_routine);
		}

#if UNITY_EDITOR
		class EditorCoroutineRunner
		{
			interface IWaitItem { bool Wait(); }
			class WaitItem_WaitForEndOfFrame : IWaitItem
			{
				public bool Wait() { mCalled = !mCalled; return mCalled; }
				private bool mCalled;
			}
			class WaitItem_WaitForSeconds : IWaitItem
			{
				public WaitItem_WaitForSeconds(float _duration) { mResumeTime = Time.realtimeSinceStartup + _duration; }
				public bool Wait() { return Time.realtimeSinceStartup < mResumeTime; }
				private float mResumeTime;
			}

			public EditorCoroutineRunner(IEnumerator _routine)
			{
				mRoutine = _routine;
				UnityEditor.EditorApplication.update += update;
			}

			private void update()
			{
				if (mWaitItem != null)
				{
					if (mWaitItem.Wait())
						return;

					mWaitItem = null;
				}

				// Stop
				if (!mRoutine.MoveNext())
				{
					UnityEditor.EditorApplication.update -= update;
					return;
				}

				// WaitForSeconds
				var waitForSeconds = mRoutine.Current as WaitForSeconds;
				if (waitForSeconds != null)
				{
					var type = waitForSeconds.GetType();
					var fieldInfo = type.GetField("m_Seconds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
					if (fieldInfo == null)
						return;

					var duration = (float)fieldInfo.GetValue(waitForSeconds);
					mWaitItem = new WaitItem_WaitForSeconds(duration);
					return;
				}

				// WaitForEndOfFrame
				var waitForEndOfFrame = mRoutine.Current as WaitForEndOfFrame;
				if (waitForEndOfFrame != null)
				{
					mWaitItem = new WaitItem_WaitForEndOfFrame();
					return;
				}
			}

			private readonly IEnumerator mRoutine;
			private IWaitItem mWaitItem;
		}
#endif

		private static CoroutineRunner sRunner;
	}
}