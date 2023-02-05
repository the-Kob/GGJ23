using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Cinemachine.DocumentationSortingAttribute;

public class EnemySpawner : MonoBehaviour
{
    private Dictionary<int, ObjectPool> enemyObjectPools = new Dictionary<int, ObjectPool>();
    private NavMeshTriangulation triangulation;

    public Transform player;
    public int numberOfEnemiesToSpawn = 5;
    public float spawnDelay = 1f;
    public List<EnemyScriptableObject> enemies = new List<EnemyScriptableObject>();
    public ScalingScriptableObject scaling;
    public SpawnMethod enemySpawnMethod = SpawnMethod.RoundRobin;
    public bool continuousSpawning;

    [Space]
    [Header("Read At Runtime")]
    [SerializeField]
    private int round = 0;
    [SerializeField]
    private List<EnemyScriptableObject> scaledEnemies = new List<EnemyScriptableObject>();

    private int enemiesAlive = 0;
    private int spawnedEnemies = 0;
    private int initialEnemiesToSpawn;
    private float initialSpawnDelay;

    public enum SpawnMethod
    {
        RoundRobin, // cycles trough the enemy pool
        Random // random enemy from the enemy pool
    }

    private void Awake()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            enemyObjectPools.Add(i, ObjectPool.CreateInstance(enemies[i].prefab, numberOfEnemiesToSpawn));
        }

        initialEnemiesToSpawn = numberOfEnemiesToSpawn;
        initialSpawnDelay = spawnDelay;
    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();

        for (int i = 0; i < enemies.Count; i++)
        {
            scaledEnemies.Add(enemies[i].ScaleUpForLevel(scaling, 0));
        }

        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        round++;
        spawnedEnemies = 0;
        enemiesAlive = 0;

        for (int i = 0; i < enemies.Count; i++)
        {
            scaledEnemies[i] = enemies[i].ScaleUpForLevel(scaling, round);
        }

        WaitForSeconds wait = new WaitForSeconds(spawnDelay);

        while(spawnedEnemies < numberOfEnemiesToSpawn)
        {
            switch(enemySpawnMethod)
            {
                case SpawnMethod.RoundRobin:
                    SpawnRoundRobinEnemy(spawnedEnemies);
                    break;
                case SpawnMethod.Random:
                    SpawnRandomEnemy();
                    break;
            }

            spawnedEnemies++;

            yield return wait;
        }

        if (continuousSpawning)
        {
            ScaleUpSpawns();
            StartCoroutine(SpawnEnemies());
        }
    }

    private void SpawnRoundRobinEnemy(int spawnedEnemies)
    {
        int spawnIndex = spawnedEnemies % enemies.Count;

        DoSpawnEnemy(spawnIndex, ChooseRandomPositionOnNavMesh());
    }

    private void SpawnRandomEnemy()
    {
        DoSpawnEnemy(Random.Range(0, enemies.Count), ChooseRandomPositionOnNavMesh());
    }

    private Vector3 ChooseRandomPositionOnNavMesh()
    {
        int vertexIndex = Random.Range(0, triangulation.vertices.Length);
        return triangulation.vertices[vertexIndex];
    }

    private void DoSpawnEnemy(int spawnIndex, Vector3 spawnPosition)
    {
        PoolableObject poolableObject = enemyObjectPools[spawnIndex].GetObject();

        if(poolableObject != null)
        {
            Enemy enemy = poolableObject.GetComponent<Enemy>();
            scaledEnemies[spawnIndex].SetupEnemy(enemy);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnPosition, out hit, 2f, -1))
            {
                enemy.agent.Warp(hit.position);
                enemy.movement.target = player;
                enemy.agent.enabled = true;
                enemy.movement.StartChasing();
                enemy.OnDie += HandleEnemyDeath;

                enemiesAlive++;
            }
            else
            {
                Debug.LogError($"Unable to place NavMeshAgent on NavMesh. Tried to use {spawnPosition}");
            }
        } else
        {
            Debug.LogError($"Unable to fetch enemy of type {spawnIndex} from object pool. Out of objects?");
        }
    }

    private void ScaleUpSpawns()
    {
        numberOfEnemiesToSpawn = Mathf.FloorToInt(initialEnemiesToSpawn * scaling.spawnCountCurve.Evaluate(round + 1));
        // spawnDelay = initialSpawnDelay * scaling.spawnRateCurve.Evaluate(round + 1);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemiesAlive--;

        if (enemiesAlive == 0 && spawnedEnemies == numberOfEnemiesToSpawn)
        {
            ScaleUpSpawns();
            StartCoroutine(SpawnEnemies());
        }
    }
}
