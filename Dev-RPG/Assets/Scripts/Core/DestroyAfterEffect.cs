using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
