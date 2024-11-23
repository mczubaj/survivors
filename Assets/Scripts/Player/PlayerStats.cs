using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
  CharacterScriptableObject characterData;

  [HideInInspector]
  public float currentHealth;

  [HideInInspector]
  public float currentRecovery;

  [HideInInspector]
  public float currentMoveSpeed;

  [HideInInspector]
  public float currentMight;

  [HideInInspector]
  public float currentProjectileSpeed;

  [HideInInspector]
  public float currentMagnet;

  [Header("Experience/Level")]
  public int experience = 0;
  public int level = 1;
  public int experienceCap;

  [System.Serializable]
  public class LevelRange
  {
    public int startLevel;
    public int endLevel;
    public int experienceCapIncrease;
  }

  public List<LevelRange> levelRanges;

  [Header("I-Frames")]
  public float invincibilityDuration;
  float invincibilityTimer;
  bool isInvincible;

  [Header("Inventory")]
  InventoryManager inventory;

  private void Awake()
  {
    characterData = CharacterSelector.GetData();
    CharacterSelector.instance.DestroySingleton();

    inventory = GetComponent<InventoryManager>();
    inventory.AddNewWeapon(characterData.StartingWeapon);

    currentHealth = characterData.MaxHealth;
    currentRecovery = characterData.Recovery;
    currentMoveSpeed = characterData.MoveSpeed;
    currentMight = characterData.Might;
    currentProjectileSpeed = characterData.ProjectileSpeed;
    currentMagnet = characterData.Magnet;
  }

  void Start()
  {
    experienceCap = levelRanges[0].experienceCapIncrease;
  }

  private void Update()
  {
    if (invincibilityTimer > 0)
    {
      invincibilityTimer -= Time.deltaTime;
    }
    else if (isInvincible)
    {
      isInvincible = false;
    }

    HealthRegen();
  }

  public void IncreaseExperience(int amount)
  {
    experience += amount;

    LevelUpChecker();
  }

  void LevelUpChecker()
  {
    if (experience >= experienceCap)
    {
      level++;
      experience -= experienceCap;

      int experienceCapIncrease = 0;
      foreach (LevelRange range in levelRanges)
      {
        if (level >= range.startLevel && level <= range.endLevel)
        {
          experienceCapIncrease = range.experienceCapIncrease;
        }
      }

      //TODO: Why not just set experienceCap inside foreach?
      experienceCap += experienceCapIncrease;
    }
  }

  public void TakeDamage(float dmg)
  {
    if (isInvincible)
    {
      return;
    }

    currentHealth -= dmg;

    invincibilityTimer = invincibilityDuration;
    isInvincible = true;

    if (currentHealth <= 0)
    {
      Kill();
    }
  }

  public void Kill()
  {
    Debug.Log("PLAYER IS DEAD");
  }

  public void RestoreHealth(float amount)
  {
    if (currentHealth < characterData.MaxHealth)
    {
      currentHealth += amount;

      if (currentHealth > characterData.MaxHealth)
      {
        currentHealth = characterData.MaxHealth;
      }
    }
  }

  void HealthRegen()
  {
    if (currentHealth < characterData.MaxHealth)
    {
      currentHealth += currentRecovery * Time.deltaTime;

      if (currentHealth > characterData.MaxHealth)
      {
        currentHealth = characterData.MaxHealth;
      }
    }
  }
}
