using UnityEngine;

public class InstantiateSpawner : Spawner
{
    [SerializeField] private GameObject prefab;

    protected override GameObject CreateItem(Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }
}