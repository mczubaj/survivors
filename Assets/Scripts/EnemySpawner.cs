using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  [System.Serializable]
  public class Wave
  {
    public string waveName;
    public List<EnemySpawnData> enemiesToSpawn;
    public float spawnInterval; // Interval at which to spawn enemies

    [HideInInspector]
    public int currentSpawnCount; // Number of enemies already spawned in this wave

    [HideInInspector]
    public int maxSpawnCount; // Total number of enemies to spawn in this wave
  }

  [System.Serializable]
  public class EnemySpawnData
  {
    public string enemyName;
    public int maxSpawnCount; // Number of enemies of this type to spawn in this wave
    public GameObject prefab;

    [HideInInspector]
    public int currentSpawnCount; // Number of enemies of this type already spawned in this wave
  }

  public List<Wave> waves;

  [HideInInspector]
  public int currentWaveIndex;

  [Header("Spawner Attributes")]
  float spawnTimer; // Timer used to determine when to spawn next enemy
  public int enemiesAliveCount;
  public int enemiesAliveLimit;
  public float waveInterval; // Interval between each wave
  public Transform minSpawnPoint,
    maxSpawnPoint; // Bottom left and top right corners just outside player's view. Used to determine enemy spawn positions

  Transform player;

  [SerializeField]
  GameObject spawnedEnemiesHolder;

  void Start()
  {
    player = FindObjectOfType<PlayerStats>().transform;
    CalculateWaveMaxSpawnCount();
    InvokeRepeating("BeginNextWave", waveInterval, waveInterval);
  }

  void Update()
  {
    spawnTimer += Time.deltaTime;

    if (spawnTimer >= waves[currentWaveIndex].spawnInterval)
    {
      spawnTimer = 0f;
      SpawnEnemies();
    }

    // Can also be achieved by making Enemy Spawner a child of Player object. But that'd make hierarchy view unclear
    transform.position = player.position;
  }

  void BeginNextWave()
  {
    if (currentWaveIndex < waves.Count - 1)
    {
      currentWaveIndex++;
      CalculateWaveMaxSpawnCount();
    }
    else
    {
      CancelInvoke("BeginNextWave");
    }
  }

  void CalculateWaveMaxSpawnCount()
  {
    int maxSpawnCount = 0;

    foreach (var enemy in waves[currentWaveIndex].enemiesToSpawn)
    {
      maxSpawnCount += enemy.maxSpawnCount;
    }

    waves[currentWaveIndex].maxSpawnCount = maxSpawnCount;
  }

  void SpawnEnemies()
  {
    Wave currentWave = waves[currentWaveIndex];

    // This early return allows some minor enemy count 'spillover', because the foreach below can spawn MULTIPLE enemies.
    // So e.g. if enemiesAliveCount == 99 and enemiesAliveLimit == 100, the foreach will run and spawn ALL the enemies that have unused spawns left, potentially exceeding the limit.
    // It's not a big issue as is. Unless some waves will have high 10s of different enemy types. Spawn system will be revamped anyway when MINIMUM spawned enemies is implemented.
    if (
      currentWave.currentSpawnCount >= currentWave.maxSpawnCount
      || enemiesAliveCount >= enemiesAliveLimit
    )
      return;

    foreach (var enemy in currentWave.enemiesToSpawn)
    {
      if (enemy.currentSpawnCount >= enemy.maxSpawnCount)
        return;

      Instantiate(
        enemy.prefab,
        SelectSpawnPoint(),
        Quaternion.identity,
        spawnedEnemiesHolder.transform
      );

      enemy.currentSpawnCount++;
      currentWave.currentSpawnCount++;
      enemiesAliveCount++;
    }
  }

  /// <summary>
  /// Returns a random point along a randomly selected screen edge outside of player's view
  /// </summary>
  public Vector2 SelectSpawnPoint()
  {
    Vector2 spawnPoint = Vector2.zero;
    string[] spawnEdges = { "top", "bottom", "right", "left" };
    string spawnEdge = spawnEdges[Random.Range(0, spawnEdges.Length)];
    float randomX = Random.Range(minSpawnPoint.position.x, maxSpawnPoint.position.x);
    float randomY = Random.Range(minSpawnPoint.position.y, maxSpawnPoint.position.y);

    switch (spawnEdge)
    {
      case "top":
        spawnPoint = new Vector2(randomX, maxSpawnPoint.position.y);
        break;
      case "right":
        spawnPoint = new Vector2(maxSpawnPoint.position.x, randomY);
        break;
      case "bottom":
        spawnPoint = new Vector2(randomX, minSpawnPoint.position.y);
        break;
      case "left":
        spawnPoint = new Vector2(minSpawnPoint.position.x, randomY);
        break;
    }

    return spawnPoint;
  }

  public void OnEnemyKilled()
  {
    enemiesAliveCount--;
  }

  private void OnDestroy()
  {
    CancelInvoke("BeginNextWave");
  }
}
