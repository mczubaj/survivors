using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
  public List<GameObject> terrainChunks;
  public GameObject chunkHolder;
  public GameObject currentChunk;

  public float checkerRadius;
  public LayerMask terrainMask;

  public GameObject player;
  PlayerMovement pm;

  [Header("Optimization")]
  public List<GameObject> spawnedChunks;
  GameObject latestChunk;
  public float maxOptimizationDistance;
  float optimizationDistance;
  float optimizerCooldown;
  public float optimizerCooldownDuration;

  void Start()
  {
    pm = FindObjectOfType<PlayerMovement>();
  }

  void Update()
  {
    ChunkChecker();
    ChunkOptimizer();
  }

  void ChunkChecker()
  {
    if (!currentChunk)
    {
      return;
    }

    if (pm.moveDir.x > 0 && pm.moveDir.y == 0)
    {
      ChunkSpawner(currentChunk.transform.Find("Right").position);
    }
    else if (pm.moveDir.x < 0 && pm.moveDir.y == 0)
    {
      ChunkSpawner(currentChunk.transform.Find("Left").position);
    }
    else if (pm.moveDir.x == 0 && pm.moveDir.y > 0)
    {
      ChunkSpawner(currentChunk.transform.Find("Up").position);
    }
    else if (pm.moveDir.x == 0 && pm.moveDir.y < 0)
    {
      ChunkSpawner(currentChunk.transform.Find("Down").position);
    }
    else if (pm.moveDir.x > 0 && pm.moveDir.y > 0)
    {
      ChunkSpawner(currentChunk.transform.Find("Right Up").position);
    }
    else if (pm.moveDir.x > 0 && pm.moveDir.y < 0)
    {
      ChunkSpawner(currentChunk.transform.Find("Right Down").position);
    }
    else if (pm.moveDir.x < 0 && pm.moveDir.y > 0)
    {
      ChunkSpawner(currentChunk.transform.Find("Left Up").position);
    }
    else if (pm.moveDir.x < 0 && pm.moveDir.y < 0)
    {
      ChunkSpawner(currentChunk.transform.Find("Left Down").position);
    }
  }

  void ChunkSpawner(Vector3 pointToCheck)
  {
    if (!Physics2D.OverlapCircle(pointToCheck, checkerRadius, terrainMask))
    {
      int rand = Random.Range(0, terrainChunks.Count);
      latestChunk = Instantiate(
        terrainChunks[rand],
        pointToCheck,
        Quaternion.identity,
        chunkHolder.transform
      );
      spawnedChunks.Add(latestChunk);
    }
  }

  void ChunkOptimizer()
  {
    optimizerCooldown -= Time.deltaTime;

    if (optimizerCooldown <= 0f)
    {
      optimizerCooldown = optimizerCooldownDuration;
    }
    else
    {
      return;
    }

    foreach (GameObject chunk in spawnedChunks)
    {
      optimizationDistance = Vector3.Distance(player.transform.position, chunk.transform.position);

      if (optimizationDistance > maxOptimizationDistance)
      {
        chunk.SetActive(false);
      }
      else
      {
        chunk.SetActive(true);
      }
    }
  }
}
