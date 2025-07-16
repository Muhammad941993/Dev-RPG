using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    private ParticleSystem _particleSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_particleSystem.IsAlive()) return;


        Destroy(objectToDestroy ? objectToDestroy : gameObject);
    }
}