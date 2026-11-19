using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledSpawner : Spawner
{
    [SerializeField] private GameObject prefab;
    [Min(1)] public int maxPoolSize = 50;

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
            false, 10, maxPoolSize);
    }

    protected override void Update()
    {
        base.Update();

        while (active.Count > 0 && active.Peek().releaseTime <= Time.time)
        {
            pool.Release(active.Dequeue().obj);
        }
    }

    protected override GameObject CreateItem(Vector3 position, Quaternion rotation)
    {
        if (pool.CountInactive == 0 && active.Count >= maxPoolSize)
        {
            pool.Release(active.Dequeue().obj);
        }

        GameObject item = pool.Get();
        item.transform.SetPositionAndRotation(position, rotation);
        return item;
    }

    protected override void ScheduleRemoval(GameObject item)
    {
        active.Enqueue(new ActiveItem { obj = item, releaseTime = Time.time + lifetime });
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