using UnityEngine;

public class normalSpawner : MonoBehaviour
{
    public GameObject prefab;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, 0.005f);
    }

    void Spawn()
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(obj, 5f);
    }
}