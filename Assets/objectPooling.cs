using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


// USED https://docs.unity.com/en-us/engine/6000.0/script-reference/unityengine/pool/objectpool1 FOR REFERENCE
public class PooledSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float spawnInterval = 0.005f;
    public float lifetime = 5f;

    // cap on the total number of objects this pool will ever create.
    [Min(1)] public int maxPoolSize = 50;

    public bool collectionChecks = false;

    struct ActiveItem
    {
        public GameObject obj;
        public float releaseTime;
    }

    IObjectPool<GameObject> pool;
    Queue<ActiveItem> active = new Queue<ActiveItem>();

    void Awake()
    {
        pool = new ObjectPool<GameObject>(
            CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
            collectionChecks, 10, maxPoolSize);
    }

    void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, spawnInterval);
    }

    void Update()
    {
        while (active.Count > 0 && active.Peek().releaseTime <= Time.time)
        {
            pool.Release(active.Dequeue().obj);
        }
    }

    void Spawn()
    {
        if (pool.CountInactive == 0 && active.Count >= maxPoolSize)
        {
            pool.Release(active.Dequeue().obj);
        }

        GameObject obj = pool.Get();
        obj.transform.position = transform.position;
        active.Enqueue(new ActiveItem { obj = obj, releaseTime = Time.time + lifetime });
    }

    GameObject CreatePooledItem()
    {
        return Instantiate(prefab);
    }

    void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }
}