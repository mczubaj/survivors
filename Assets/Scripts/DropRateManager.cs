using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
  const float maxDropRate = 100f;

  [System.Serializable]
  public class Drop
  {
    public string name;
    public GameObject itemPrefab;

    [Range(0f, maxDropRate)]
    public float dropRate;
  }

  public List<Drop> drops;

  void OnDestroy()
  {
    //TODO: Not sure if this early return is a good practice. Should probably handle creating the drop in Kill() on Enemy rather than here.
    // This early return prevents an error with scene cleanup. Without it, drops remain on scene after unloading the scene.
    if (!gameObject.scene.isLoaded)
      return;

    Drop drop = GetRarestRandomDrop();

    if (drop != null)
    {
      Instantiate(
        drop.itemPrefab,
        transform.position,
        Quaternion.identity,
        PickupHolder.Instance.transform
      );
    }
  }

  /// <summary>
  /// Rolls for drops and returns the rarest rolled one or null if none are rolled
  /// </summary>
  /// <returns>DropRateManager.Drop || null</returns>
  Drop GetRarestRandomDrop()
  {
    float randomNumber = Random.Range(0f, maxDropRate);
    float minDropRate = maxDropRate;
    Drop rarestDrop = null;

    foreach (var drop in drops)
    {
      if (randomNumber <= drop.dropRate && drop.dropRate <= minDropRate)
      {
        minDropRate = drop.dropRate;
        rarestDrop = drop;
      }
    }

    return rarestDrop;
  }
}
