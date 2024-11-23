using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
  [SerializeField]
  int inventorySlotsLimit = 6;

  [SerializeField]
  List<InventoryItemWeapon> weaponSlots = new List<InventoryItemWeapon>();

  [SerializeField]
  List<InventoryItemPassive> passiveItemSlots = new List<InventoryItemPassive>();

  public void AddNewWeapon(WeaponScriptableObject weapon)
  {
    if (weaponSlots.Count >= inventorySlotsLimit)
    {
      Debug.LogError($"Tried adding {weapon.name} to inventory but all weapon slots are full");
      return;
    }

    WeaponController spawnedController = Instantiate(weapon.WeaponController, transform);
    spawnedController.UpdateStats(weapon.WeaponDataPerLevel[0]);

    InventoryItemWeapon inventoryItem = new InventoryItemWeapon(
      spawnedController,
      weapon.WeaponDataPerLevel
    );
    weaponSlots.Add(inventoryItem);
  }

  public void LevelUpWeapon(int slotIndex)
  {
    if (slotIndex >= weaponSlots.Count)
    {
      Debug.LogError($"Tried upgrading weapon at slot {slotIndex}, but the slot is empty");
      return;
    }

    InventoryItemWeapon invWeapon = weaponSlots[slotIndex];

    if (invWeapon.currentLevel >= invWeapon.weaponDataPerLevel.Count)
    {
      Debug.LogError(
        $"Tried upgrading weapon {invWeapon.spawnedWeaponController.name}, but there are no more upgrade levels defined"
      );
      return;
    }

    WeaponScriptableObject.WeaponData upgradedWeaponData = invWeapon.weaponDataPerLevel[
      invWeapon.currentLevel
    ];

    invWeapon.spawnedWeaponController.UpdateStats(upgradedWeaponData);
    invWeapon.spawnedWeaponController.OnUpgrade();
    invWeapon.currentLevel++;
  }

  public void AddNewPassiveItem(PassiveItemScriptableObject item)
  {
    if (passiveItemSlots.Count >= inventorySlotsLimit)
    {
      Debug.LogError($"Tried adding {item.name} to inventory but all passive item slots are full");
      return;
    }

    PassiveItem spawnedItem = Instantiate(item.PassiveItem, transform);
    spawnedItem.UpdateStats(item.PassiveItemDataPerLevel[0]);

    InventoryItemPassive inventoryItem = new InventoryItemPassive(
      spawnedItem,
      item.PassiveItemDataPerLevel
    );
    passiveItemSlots.Add(inventoryItem);
  }

  public void LevelUpPassiveItem(int slotIndex)
  {
    if (slotIndex >= passiveItemSlots.Count)
    {
      Debug.LogError($"Tried upgrading item at slot {slotIndex}, but the slot is empty");
      return;
    }

    InventoryItemPassive invItem = passiveItemSlots[slotIndex];

    if (invItem.currentLevel >= invItem.passiveItemDataPerLevel.Count)
    {
      Debug.LogError(
        $"Tried upgrading item {invItem.spawnedPassiveItem.name}, but there are no more upgrade levels defined"
      );
      return;
    }

    PassiveItemScriptableObject.PassiveItemData upgradedItemData = invItem.passiveItemDataPerLevel[
      invItem.currentLevel
    ];

    invItem.spawnedPassiveItem.UpdateStats(upgradedItemData);
    invItem.spawnedPassiveItem.OnUpgrade();
    invItem.currentLevel++;
  }
}

// Classes for inventory item types

[System.Serializable]
public class InventoryItemWeapon
{
  public int currentLevel;
  public WeaponController spawnedWeaponController;
  public List<WeaponScriptableObject.WeaponData> weaponDataPerLevel;

  public InventoryItemWeapon(
    WeaponController spawnedWeaponController,
    List<WeaponScriptableObject.WeaponData> weaponDataPerLevel
  )
  {
    currentLevel = 1;
    this.spawnedWeaponController = spawnedWeaponController;
    this.weaponDataPerLevel = weaponDataPerLevel;
  }
}

[System.Serializable]
public class InventoryItemPassive
{
  public int currentLevel;
  public PassiveItem spawnedPassiveItem;
  public List<PassiveItemScriptableObject.PassiveItemData> passiveItemDataPerLevel;

  public InventoryItemPassive(
    PassiveItem spawnedPassiveItem,
    List<PassiveItemScriptableObject.PassiveItemData> passiveItemDataPerLevel
  )
  {
    currentLevel = 1;
    this.spawnedPassiveItem = spawnedPassiveItem;
    this.passiveItemDataPerLevel = passiveItemDataPerLevel;
  }
}
