using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
  fileName = "PassiveItemScriptableObject",
  menuName = "ScriptableObjects/Passive Item"
)]
public class PassiveItemScriptableObject : ScriptableObject
{
  [SerializeField]
  private PassiveItem passiveItem;
  public PassiveItem PassiveItem
  {
    get => passiveItem;
  }

  [Tooltip("Holds item data for each upgrade level of the passive item")]
  [SerializeField]
  private List<PassiveItemData> passiveItemDataPerLevel;
  public List<PassiveItemData> PassiveItemDataPerLevel
  {
    get => passiveItemDataPerLevel;
  }

  [Serializable]
  public class PassiveItemData
  {
    [SerializeField]
    private float multiplier;
    public float Multiplier
    {
      get => multiplier;
    }
  }
}
