using System.Collections;
using UnityEngine;

namespace aisilol
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

        private MonoBehaviour mRunner;
        private IEnumerator mRoutine;
    }
}