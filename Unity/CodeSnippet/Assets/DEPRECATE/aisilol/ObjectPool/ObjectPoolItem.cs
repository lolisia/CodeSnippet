using UnityEngine;

namespace aisilol
{
    public class ObjectPoolItem : MonoBehaviour
    {
        public bool Activate
        {
            get { return mActivate; }
            set { mActivate = value; gameObject.SetActive(value); }
        }

        public void Init(ObjectPoolManager _manager, GameObject _prefab)
        {
            mManager = _manager;
            mPrefab = _prefab;

            inactive();
        }
        public void Spawn()
        {
            active();
        }
        public void Despawn()
        {
            mManager.Push(mPrefab, gameObject);
            inactive();
        }

        private void active()
        {
            Activate = true;
            transform.parent = null;
        }
        private void inactive()
        {
            if (mManager == null)
                return;

            transform.parent = mManager.transform;
            name = string.Format("[Pooled Item] {0}.prefab", mPrefab.name);

            Activate = false;
        }

        private ObjectPoolManager mManager;
        private GameObject mPrefab;
        private bool mActivate;
    }
}