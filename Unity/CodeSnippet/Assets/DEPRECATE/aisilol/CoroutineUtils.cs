using System.Collections;
using UnityEngine;

namespace aisilol_Deprecate
{
    public class SingleCoroutineRunner
    {
        public SingleCoroutineRunner(MonoBehaviour _runner)
        {
            mRunner = _runner;
        }
        public void Run(IEnumerator _routine)
        {
            if (mRunner == null)
                return;

            Stop();

            mRoutine = _routine;
            mRunner.StartCoroutine(mRoutine);
        }
        public void Stop()
        {
            if (mRoutine == null)
                return;

            mRunner.StopCoroutine(mRoutine);
            mRoutine = null;
        }

        private readonly MonoBehaviour mRunner;
        private IEnumerator mRoutine;
    }
}