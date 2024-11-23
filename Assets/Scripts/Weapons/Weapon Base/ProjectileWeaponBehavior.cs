using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehavior : MonoBehaviour
{
  WeaponScriptableObject.WeaponData weaponData;

  public float destroyAfterSeconds;

  protected float currentDamage;
  protected float currentSpeed;
  protected float currentCooldownDuration;
  protected int currentPierce;

  protected Vector3 direction;

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

  //TODO: Consider finding PlayerStats in WeaponController instead of below
  // Then initialize it in Awake using transform.parent and also multiply weaponData.Damage by might in Awake
  // Should be less expensive because WeaponController instantiates MUCH less often and allows removing the consufing GetCurrentDamage
  public float GetCurrentDamage()
  {
    return currentDamage * FindObjectOfType<PlayerStats>().currentMight;
  }

  public void DirectionChecker(Vector3 dir)
  {
    direction = dir;

    float dirx = direction.x;
    float diry = direction.y;

    Vector3 scale = transform.localScale;
    Vector3 rotation = transform.root.eulerAngles;

    rotation.z = -45f;

    // TODO: alternative to the ifology - https://blog.terresquall.com/community/topic/part-3-different-way-to-handle-shooting/#post-11833
    // Or at least handle it with switch case like in EnemySpawner

    //TODO: fix the flips/rotations, they are incorrect e.g. for left (item rotated instead of flipped)

    if (dirx < 0 && diry == 0) // left
    {
      scale.x = scale.x * -1;
      scale.y = scale.y * -1;
    }
    else if (dirx == 0 && diry > 0) // up
    {
      scale.x = scale.x * -1;
    }
    else if (dirx == 0 && diry < 0) // down
    {
      scale.y = scale.y * -1;
    }
    else if (dirx > 0 && diry > 0) // right up
    {
      rotation.z = 0f;
    }
    else if (dirx > 0 && diry < 0) // right down
    {
      rotation.z = -90f;
    }
    else if (dirx < 0 && diry > 0) // left up
    {
      scale.x = scale.x * -1;
      scale.y = scale.y * -1;
      rotation.z = -90f;
    }
    else if (dirx < 0 && diry < 0) // left down
    {
      scale.x = scale.x * -1;
      scale.y = scale.y * -1;
      rotation.z = 0f;
    }

    transform.localScale = scale;
    transform.rotation = Quaternion.Euler(rotation);
  }

  protected virtual void OnTriggerEnter2D(Collider2D col)
  {
    if (col.CompareTag("Enemy"))
    {
      EnemyStats enemy = col.GetComponent<EnemyStats>();
      enemy.TakeDamage(GetCurrentDamage());
      reducePierce();
    }
    else if (col.CompareTag("Prop"))
    {
      if (col.gameObject.TryGetComponent(out BreakableProps breakable))
      {
        breakable.TakeDamage(GetCurrentDamage());
        reducePierce();
      }
    }
  }

  void reducePierce()
  {
    currentPierce--;
    if (currentPierce <= 0)
    {
      Destroy(gameObject);
    }
  }
}
