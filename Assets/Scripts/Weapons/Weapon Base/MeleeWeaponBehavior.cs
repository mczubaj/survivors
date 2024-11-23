using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBehavior : MonoBehaviour
{
  WeaponScriptableObject.WeaponData weaponData;

  public float destroyAfterSeconds;

  protected float currentDamage;
  protected float currentSpeed;
  protected float currentCooldownDuration;
  protected int currentPierce;

  void Awake()
  {
    weaponData = transform.parent.GetComponent<WeaponController>().weaponData;

    currentDamage = weaponData.Damage;
    currentSpeed = weaponData.Speed;
    currentCooldownDuration = weaponData.CooldownDuration;
    currentPierce = weaponData.Pierce;
  }

  protected virtual void Start()
  {
    Destroy(gameObject, destroyAfterSeconds);
  }

  public float GetCurrentDamage()
  {
    return currentDamage * FindObjectOfType<PlayerStats>().currentMight;
  }

  protected virtual void OnTriggerEnter2D(Collider2D col)
  {
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
