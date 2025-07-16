using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
    private static bool _isSpawned;
    [SerializeField] private GameObject persistentObjectPrefab;

    private void Awake()
    {
        if (_isSpawned) return;
        SpawnPersistentObject();
        _isSpawned = true;
    }

    private void SpawnPersistentObject()
    {
        var persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}