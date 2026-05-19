using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public GameObject[] possibleEnemies;
    public int maxEnemies = 2;

    private int currentEnemies = 0;

    void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-4, 4),
            1,
            -6
        );

        int randomIndex = Random.Range(0, possibleEnemies.Length);
        Instantiate(possibleEnemies[randomIndex], spawnPos, Quaternion.identity);

        currentEnemies++;
        print("Spawneado");
    }

    public void EnemyDied()
    {
        currentEnemies--;

        if (currentEnemies < maxEnemies)
        {
            SpawnEnemy();
        }
    }
}
