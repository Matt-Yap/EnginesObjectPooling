using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected Transform spawnPoint;
    [SerializeField] protected float spawnInterval = 0.5f;
    [SerializeField] protected float lifetime = 5f;

    bool isSpawning;

    protected abstract GameObject CreateItem(Vector3 position, Quaternion rotation);

    protected virtual void ScheduleRemoval(GameObject item)
    {
        Destroy(item, lifetime);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSpawning)
        {
            StartSpawning();
        }

        if (Input.GetKeyDown(KeyCode.S) && isSpawning)
        {
            StopSpawning();
        }
    }

    void StartSpawning()
    {
        isSpawning = true;
        InvokeRepeating(nameof(Spawn), 0f, spawnInterval);
    }

    void StopSpawning()
    {
        isSpawning = false;
        CancelInvoke(nameof(Spawn));
    }

    void Spawn()
    {
        Transform point = spawnPoint != null ? spawnPoint : transform;

        GameObject item = CreateItem(point.position, point.rotation);
        ScheduleRemoval(item);
    }
}