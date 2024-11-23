using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
  [SerializeField]
  private WeaponController weaponController;
  public WeaponController WeaponController
  {
    get => weaponController;
  }

  [Tooltip("Holds weapon data for each upgrade level of the weapon")]
  [SerializeField]
  private List<WeaponData> weaponDataPerLevel;
  public List<WeaponData> WeaponDataPerLevel
  {
    get => weaponDataPerLevel;
  }

  [Serializable]
  public class WeaponData
  {
    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab
    {
      get => prefab;
    }

    [SerializeField]
    private float damage;
    public float Damage
    {
      get => damage;
    }

    [SerializeField]
    private float speed;
    public float Speed
    {
      get => speed;
    }

    [SerializeField]
    private float cooldownDuration;
    public float CooldownDuration
    {
      get => cooldownDuration;
    }

    [SerializeField]
    private int pierce;
    public int Pierce
    {
      get => pierce;
    }
  }
}
