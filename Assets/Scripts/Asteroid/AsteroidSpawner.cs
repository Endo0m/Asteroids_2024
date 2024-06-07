using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private Asteroid asteroidPrefab;
    [SerializeField]
    private float spawnDistance = 12f;
    [SerializeField]
    private float spawnRate = 1f;
    [SerializeField]
    private int amountPerSpawn = 1;
    [Range(0f, 45f)]
    private float trajectoryVariance = 15f;
    

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void Spawn()
    {
        for (int i = 0; i < amountPerSpawn; i++)
        {
          
            Vector3 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = transform.position + (spawnDirection * spawnDistance);

            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
                        
            Asteroid asteroid = Instantiate(asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);

            Vector2 trajectory = rotation * -spawnDirection;
            asteroid.SetTrajectory(trajectory);
        }
    }

}
