using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
  PlayerStats player;
  CircleCollider2D playerCollector;
  public float pullSpeed;

  private void Start()
  {
    player = FindObjectOfType<PlayerStats>();
    playerCollector = GetComponent<CircleCollider2D>();
  }

  private void Update()
  {
    playerCollector.radius = player.currentMagnet;
  }

  private void OnTriggerEnter2D(Collider2D col)
  {
    if (col.gameObject.TryGetComponent(out ICollectible collectible))
    {
      Rigidbody2D collectibleRb = col.gameObject.GetComponent<Rigidbody2D>();
      Vector2 forceDirection = (transform.position - col.transform.position).normalized;
      collectibleRb.AddForce(forceDirection * pullSpeed);

      collectible.Collect();
    }
  }
}
