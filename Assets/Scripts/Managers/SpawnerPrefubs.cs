using UnityEngine;

public class SpawnerPrefubs : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnPrefabs;
    [SerializeField]
    private float spawnInterval;
    [SerializeField]
    private Vector2 spawnPosMin;
    [SerializeField]
    private Vector2 spawnPosMax;
    [SerializeField]
    private float checkRadius = 1f; // Радиус для проверки перекрытия

    private void Start()
    {
        InvokeRepeating(nameof(SpawnPrefubs), spawnInterval, spawnInterval);
    }

    private void SpawnPrefubs()
    {
        int index = Random.Range(0, spawnPrefabs.Length);
        Vector2 spawnPos;
        int maxAttempts = 10; // Максимальное количество попыток для поиска свободного места
        int attempts = 0;

        do
        {
            spawnPos = new Vector2(
                Random.Range(spawnPosMin.x, spawnPosMax.x),
                Random.Range(spawnPosMin.y, spawnPosMax.y)
            );
            attempts++;
        } while (Physics2D.OverlapCircle(spawnPos, checkRadius) != null && attempts < maxAttempts);

        if (attempts < maxAttempts)
        {
            Instantiate(spawnPrefabs[index], spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Не удалось найти свободное место для спавна префаба.");
        }
    }
}