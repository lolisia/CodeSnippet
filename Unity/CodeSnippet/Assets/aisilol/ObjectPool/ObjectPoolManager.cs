using System.Collections.Generic;
using UnityEngine;

namespace aisilol
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public void Preload(GameObject _prefab, int _count)
        {
            for (var index = 0; index < _count; index++)
            {
                Push(_prefab, createInstance(_prefab));
            }
        }
        public void Push(GameObject _prefab, GameObject _instance)
        {
            var item = _instance.AddComponentOnce<ObjectPoolItem>();
            item.Init(this, _prefab);

            var list = mPrefabInstanceDic.Get(_prefab);
            list.Add(item);
        }
        public GameObject Pop(GameObject _prefab)
        {
            var list = mPrefabInstanceDic.Get(_prefab);
            foreach (var item in list)
            {
                if (item.Activate)
                    continue;

                list.Remove(item);

                item.Spawn();
                return item.gameObject;
            }

            Push(_prefab, createInstance(_prefab));
            return Pop(_prefab);
        }

        private GameObject createInstance(GameObject _prefab)
        {
            return GameObject_.Instantiate(_prefab);
        }

        private Dictionary<GameObject, List<ObjectPoolItem>> mPrefabInstanceDic = new Dictionary<GameObject, List<ObjectPoolItem>>();
    }
}