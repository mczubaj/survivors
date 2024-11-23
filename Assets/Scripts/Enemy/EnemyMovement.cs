using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  Transform player;
  EnemyStats enemy;

  void Start()
  {
    player = FindObjectOfType<PlayerMovement>().transform;
    enemy = FindObjectOfType<EnemyStats>();
  }

  void Update()
  {
    transform.position = Vector2.MoveTowards(
      transform.position,
      player.position,
      enemy.currentMoveSpeed * Time.deltaTime
    );
  }
}
