﻿using System.Collections.Generic;
using UnityEngine;

namespace aisilol_Deprecate
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


		private readonly string mName;
		private readonly float mStartTime;
	}

	public static class ScopeProfilerManager
	{
		public class ProfileInfo
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

					return Math_.Divide(Elapsed, CallCount);
				}
			}
		}

		internal static void Add(string _name, float _duration)
		{
			var info = sProfileInfoDic.Get(_name);
			info.AddElapsed(_duration);
		}
		public static void Clear()
		{
			sProfileInfoDic.Clear();
		}
		public static Dictionary<string, ProfileInfo> GetResult()
		{
			return sProfileInfoDic;
		}

		private static readonly Dictionary<string, ProfileInfo> sProfileInfoDic = new Dictionary<string, ProfileInfo>();
	}
}