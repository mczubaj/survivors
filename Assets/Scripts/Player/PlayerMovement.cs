using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [HideInInspector]
  public float lastHorizontalVector;

  [HideInInspector]
  public float lastVerticalVector;

  [HideInInspector]
  public Vector2 moveDir;

  [HideInInspector]
  public Vector2 lastMovedVector;

  Rigidbody2D rb;
  PlayerStats player;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    player = GetComponent<PlayerStats>();
    lastMovedVector = new Vector2(1, 0f); //Makes sure weapons start firing in a default direction if the player doesn't move at game start
  }

  void Update()
  {
    InputManagement();
  }

  void FixedUpdate()
  {
    Move();
  }

  void InputManagement()
  {
    float moveX = Input.GetAxisRaw("Horizontal");
    float moveY = Input.GetAxisRaw("Vertical");

    moveDir = new Vector2(moveX, moveY).normalized;

    if (moveDir.x != 0)
    {
      lastHorizontalVector = moveDir.x;
      lastMovedVector = new Vector2(lastHorizontalVector, 0f);
    }

    if (moveDir.y != 0)
    {
      lastVerticalVector = moveDir.y;
      lastMovedVector = new Vector2(0f, lastVerticalVector);
    }

    if (moveDir.x != 0 && moveDir.y != 0)
    {
      lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
    }
  }

  void Move()
  {
    rb.velocity = new Vector2(
      moveDir.x * player.currentMoveSpeed,
      moveDir.y * player.currentMoveSpeed
    );
  }
}
