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

			var instance = new GameObject("[CoroutineRunner]");
			var runner = instance.AddComponent<CoroutineRunner>();
			runner.StartCoroutine(onRun(instance, _routine));
		}

		private static IEnumerator onRun(GameObject _instance, IEnumerator _routine)
		{
			yield return _routine;
			Destroy(_instance);
		}
	}
}