using UnityEngine;

namespace aisilol
{
	public class Singleton<T> : MonoBehaviour
		where T : Singleton<T>
	{
		public static string Name { get { return string.Format("<SINGLETON> {0}", typeof(T).Name); } }
		public static T Instance
		{
			get
			{
				if (mInstance != null)
					return mInstance;

				// 이미 생성한 Instance가 1개 존재할 경우, 해당 Instance 반환
				var instances = Object.FindObjectsOfType<T>();
				if (instances.Length == 1)
				{
					mInstance = instances[0];
					mInstance.name = Name;
					return mInstance;
				}

				// 같은 Type의 Instance를 모두 제거한 후, 1개 생성
				foreach (var instance in instances)
				{
					Destroy(instance.gameObject);
				}

				var o = new GameObject(Name, typeof(T));
				mInstance = o.GetComponent<T>();
				DontDestroyOnLoad(o);

				return mInstance;
			}
		}

		private static T mInstance;
	}
}