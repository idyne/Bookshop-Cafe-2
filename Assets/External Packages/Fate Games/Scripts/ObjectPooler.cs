using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        private class PoolData
        {
            public string tag;
            public GameObject prefab;
            public int size;
            public bool canActiveObjectsBeDequeued = false;
        }
        private struct Pool
        {
            private Queue<GameObject> instances;
            private bool canActiveObjectsBeDequeued;

            public Pool(Queue<GameObject> instances, bool canActiveObjectsBeDequeued)
            {
                this.instances = instances;
                this.canActiveObjectsBeDequeued = canActiveObjectsBeDequeued;
            }

            public Queue<GameObject> Instances { get => instances; }
            public bool CanActiveObjectsBeDequeued { get => canActiveObjectsBeDequeued; }
        }
        public static ObjectPooler Instance;
        private void Awake()
        {
            Instance = this;
            CreatePools();
        }
        [SerializeField] private List<PoolData> pools;
        private Dictionary<string, Pool> poolDictionary;
        public void CreatePools()
        {
            poolDictionary = new Dictionary<string, Pool>();
            foreach (PoolData poolData in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                Transform poolObj = new GameObject(poolData.tag + " Pool").transform;
                poolObj.transform.position = Vector3.up * 100;
                for (int i = 0; i < poolData.size; i++)
                {
                    GameObject obj = Instantiate(poolData.prefab, poolObj);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                Pool pool = new Pool(objectPool, poolData.canActiveObjectsBeDequeued);
                poolDictionary.Add(poolData.tag, pool);
            }
        }
        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }
            GameObject objectToSpawn;
            Pool pool = poolDictionary[tag];
            objectToSpawn = pool.Instances.Dequeue();
            pool.Instances.Enqueue(objectToSpawn);
            if (!pool.CanActiveObjectsBeDequeued)
            {
                
                int i = pool.Instances.Count;
                while (i-- > 0 && objectToSpawn.activeSelf)
                {
                    objectToSpawn = pool.Instances.Dequeue();
                    pool.Instances.Enqueue(objectToSpawn);
                }
                if (objectToSpawn.activeSelf)
                {
                    Debug.LogError("All instances of \"" + tag + "\" are currently active!");
                    return null;
                }
            }
            DOTween.Kill(objectToSpawn);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);
            IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
            if (pooledObj != null)
                pooledObj.OnObjectSpawn();
            return objectToSpawn;
        }
    }
}