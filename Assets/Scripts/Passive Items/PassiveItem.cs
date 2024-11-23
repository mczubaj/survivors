using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
  protected PlayerStats player;
  public PassiveItemScriptableObject.PassiveItemData passiveItemData;

  void Start()
  {
    player = FindObjectOfType<PlayerStats>();
    ApplyModifier();
  }

  //TODO: https://trello.com/c/kDFsfzbk/31-switch-weapon-and-passiveitem-controllers-from-activate-deactivate-to-instantiate-destroy
  public void UpdateStats(PassiveItemScriptableObject.PassiveItemData upgradedItemData)
  {
    gameObject.SetActive(false);
    passiveItemData = upgradedItemData;
    gameObject.SetActive(true);
  }

  public void OnUpgrade()
  {
    ApplyModifier();
  }

  //TODO: Working with the multiplier can be weird. When upgrading, it gets applied on top of the previous value.
  // E.g. base speed = 5; with wings with 50 multiplier applied it gets to 7.5; but when wings are upgraded to 75 multiplier, speed now = 13.125
  // Consider different approaches for more convenient and clearer balancing
  protected virtual void ApplyModifier() { }
}
