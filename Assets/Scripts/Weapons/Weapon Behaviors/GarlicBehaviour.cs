using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarlicBehaviour : MeleeWeaponBehavior
{
  List<GameObject> markedEnemies;

  protected override void Start()
  {
    base.Start();
    markedEnemies = new List<GameObject>();
  }

  protected override void OnTriggerEnter2D(Collider2D col)
  {
    if (markedEnemies.Contains(col.gameObject))
    {
      return;
    }
    else
    {
      markedEnemies.Add(col.gameObject);
    }

    if (col.CompareTag("Enemy"))
    {
      EnemyStats enemy = col.GetComponent<EnemyStats>();
      enemy.TakeDamage(GetCurrentDamage());
    }
    else if (col.CompareTag("Prop"))
    {
      if (col.gameObject.TryGetComponent(out BreakableProps breakable))
      {
        breakable.TakeDamage(GetCurrentDamage());
      }
    }
  }
}
