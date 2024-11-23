using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
  public EnemyScriptableObject enemyData;

  [HideInInspector]
  public float currentMoveSpeed;

  [HideInInspector]
  public float currentHealth;

  [HideInInspector]
  public float currentDamage;

  public float despawnDistance = 20f;
  private float despawnCheckInterval = 1f;
  private float despawnCheckInitialDelay = 3f;

  Transform player;
  EnemySpawner enemySpawner;

  void Awake()
  {
    currentMoveSpeed = enemyData.MoveSpeed;
    currentHealth = enemyData.MaxHealth;
    currentDamage = enemyData.Damage;
  }

  private void Start()
  {
    player = FindObjectOfType<PlayerStats>().transform;
    enemySpawner = FindObjectOfType<EnemySpawner>();
    InvokeRepeating("DespawnChecker", despawnCheckInitialDelay, despawnCheckInterval);
  }

  public void TakeDamage(float dmg)
  {
    currentHealth -= dmg;

    if (currentHealth <= 0)
    {
      Kill();
    }
  }

  public void Kill()
  {
    enemySpawner.OnEnemyKilled();
    Destroy(gameObject);
  }

  private void OnCollisionStay2D(Collision2D col)
  {
    if (col.gameObject.CompareTag("Player"))
    {
      PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
      player.TakeDamage(currentDamage);
    }
  }

  /// <summary>
  /// Checks if enemy is further away from player than despawnDistance and moves the enemy close to player if true
  /// </summary>
  private void DespawnChecker()
  {
    if (Vector2.Distance(transform.position, player.position) >= despawnDistance)
    {
      transform.position = enemySpawner.SelectSpawnPoint();
    }
  }

  private void OnDestroy()
  {
    CancelInvoke("DespawnChecker");
  }
}
