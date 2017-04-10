using System;
using System.Collections.Generic;
using UnityEngine;

namespace aisilol
{
	public class ScopeProfiler : Scope
	{
		public ScopeProfiler(string _name)
		{
			mName = _name;
			mStartTime = Time.realtimeSinceStartup;
		}
		protected override void CloseScope()
		{
			ScopeProfilerManager.Add(mName, Time.realtimeSinceStartup - mStartTime);
		}


		private string mName;
		private float mStartTime;
	}

	static class ScopeProfilerManager
	{
		class ProfileInfo
		{
			public int CallCount { get; private set; }
			public float Elapsed { get; private set; }

			public void AddElapsed(float _elapsed)
			{
				CallCount++;
				Elapsed += _elapsed;
			}

			public float Average
			{
				get
				{
					if (CallCount == 0)
						return Elapsed;

					return MathUtils.Divide(Elapsed, CallCount);
				}
			}
		}

		public static void Add(string _name, float _duration)
		{
			var info = sProfileInfoDic.Get(_name);
			info.AddElapsed(_duration);
		}
		public static void Clear()
		{
			sProfileInfoDic.Clear();
		}

		private static Dictionary<string, ProfileInfo> sProfileInfoDic = new Dictionary<string, ProfileInfo>();
	}
}