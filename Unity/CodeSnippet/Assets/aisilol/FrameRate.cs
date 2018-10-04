using UnityEngine;

namespace aisilol
{
	public class FrameRate : MonoBehaviour
	{
		public float Interval = 1f;
		public bool UseGUI;

		public float Current { get; private set; }
		public float Average { get; private set; }
		public float Min { get; private set; }
		public float Max { get; private set; }


		void Start()
		{
			mTimeLeft = Interval;

			Min = float.MaxValue;
			Max = -1;

			mBoldLabel = new GUIStyle("label") { fontStyle = FontStyle.Bold };
		}
		void Update()
		{
			mTimeLeft -= Time.deltaTime;
			mAccumulate += Math_.Divide(Time.timeScale, Time.deltaTime);
			mFrameCount++;

			if (0 < mTimeLeft)
				return;

			mItemCount++;
			Current = Math_.Divide(mAccumulate, mFrameCount);
			Average = Math_.Divide(((mItemCount - 1) * Average + Current), mItemCount);

			Min = Mathf.Min(Min, Current);
			Max = Mathf.Max(Max, Current);

			mTimeLeft = Interval;
			mAccumulate = 0;
			mFrameCount = 0;
		}

		void OnGUI()
		{
			if (!UseGUI)
				return;

			var height = 18;
			var offset = 5;

			GUI.Box(new Rect(20 - offset, 20 - offset, 100 + offset * 2, height * 4 + offset * 2), "");
			using (new GUI.GroupScope(new Rect(20, 20, 100, height * 4)))
			{
				using (new GUI_.ColorScope(Mathf.Approximately(Current, Min) ? Color.red : Color.green))
				{
					GUI.Label(new Rect(0, 0, 100, height), string.Format("{0} FPS", Current.ToString("f2")), mBoldLabel);
				}

				GUI.Label(new Rect(0, height * 1, 100, height), string.Format("{0} AVERAGE", Average.ToString("f2")));

				using (new GUI_.ColorScope(Color.red))
				{
					var min = Max < Min ? 0 : Min;
					GUI.Label(new Rect(0, height * 2, 100, height), string.Format("{0} MIN", min.ToString("f2")));
				}
				using (new GUI_.ColorScope(Color.green))
				{
					GUI.Label(new Rect(0, height * 3, 100, height), string.Format("{0} MAX", Max.ToString("f2")));
				}
			}
		}


		private float mTimeLeft;
		private float mAccumulate;
		private float mFrameCount;

		private int mItemCount;

		private GUIStyle mBoldLabel;
	}
}