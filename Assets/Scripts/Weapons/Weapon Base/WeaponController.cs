using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base script for all weapon controllers
/// </summary>

public class WeaponController : MonoBehaviour
{
  [HideInInspector]
  public WeaponScriptableObject.WeaponData weaponData;
  float currentCooldown;

  protected PlayerMovement pm;

  protected virtual void Start()
  {
    pm = FindObjectOfType<PlayerMovement>();
  }

  protected virtual void Update()
  {
    currentCooldown -= Time.deltaTime;
    if (currentCooldown <= 0f)
    {
      Attack();
    }
  }

  protected virtual void Attack()
  {
    currentCooldown = weaponData.CooldownDuration;
  }

  //TODO: https://trello.com/c/kDFsfzbk/31-switch-weapon-and-passiveitem-controllers-from-activate-deactivate-to-instantiate-destroy
  public void UpdateStats(WeaponScriptableObject.WeaponData upgradedWeaponData)
  {
    gameObject.SetActive(false);
    weaponData = upgradedWeaponData;
    currentCooldown = weaponData.CooldownDuration;
    gameObject.SetActive(true);
  }

  public void OnUpgrade()
  {
    //TODO: Implement a timer that will prevent spamming attack when multiple upgrades are applied in a short time
    Attack();
  }
}
