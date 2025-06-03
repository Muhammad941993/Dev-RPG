using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject persistentObjectPrefab;
    
    private static bool _isSpawned;
    private void Awake()
    {
        if (_isSpawned) return;
        SpawnPersistentObject();
        _isSpawned = true;
    }

    private void SpawnPersistentObject()
    {
        GameObject persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}
