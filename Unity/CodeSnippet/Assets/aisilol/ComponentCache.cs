using System.Collections.Generic;
using UnityEngine;

namespace aisilol
{
	public class ComponentCache
	{
		public ComponentCache(GameObject _parent)
		{
			mGameObject = _parent;
		}
		public T Get<T>() where T : Component
		{
			var name = typeof(T).Name;
			if (mComponentCacheDic.Find(name))
			{
				var value = mComponentCacheDic[name];

				// Cache 대상이 없다면, Cache에서 제거한다.
				if (value == null)
					mComponentCacheDic.Remove(name);

				return value as T;
			}

			var component = mGameObject.GetComponent<T>();
			if (component == null)
				return null;

			mComponentCacheDic.Add(name, component);
			return component;
		}


		private readonly GameObject mGameObject;
		private readonly Dictionary<string, Component> mComponentCacheDic = new Dictionary<string, Component>();
	}
}