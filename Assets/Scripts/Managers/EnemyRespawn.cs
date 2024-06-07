using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemySpawnPrefabs;
    [SerializeField]
    private float initialSpawnInterval = 15f;
    [SerializeField]
    private float minSpawnInterval = 6f;
    [SerializeField]
    private float intervalDecreaseStep = 3f;
    [SerializeField]
    private float intervalDecreaseTime = 30f;
    [SerializeField]
    private Vector2 spawnPosMin;
    [SerializeField]
    private Vector2 spawnPosMax;

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        float currentInterval = initialSpawnInterval;
        float elapsedTime = 0f;

        while (true)
        {
            EnemySpawnPrefubs();
            yield return new WaitForSeconds(currentInterval);

            elapsedTime += currentInterval;
            if (elapsedTime >= intervalDecreaseTime && currentInterval > minSpawnInterval)
            {
                currentInterval = Mathf.Max(minSpawnInterval, currentInterval - intervalDecreaseStep);
                elapsedTime = 0f; // Reset elapsed time after each decrease
            }
        }
    }

    private void EnemySpawnPrefubs()
    {
        int index = Random.Range(0, enemySpawnPrefabs.Length);
        Vector2 spawnPos = new Vector2(
            Random.Range(spawnPosMin.x, spawnPosMax.x),
            Random.Range(spawnPosMin.y, spawnPosMax.y)
        );

        Instantiate(enemySpawnPrefabs[index], spawnPos, Quaternion.identity);
    }
}