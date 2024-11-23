using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Create a common reusable class for singletons, instead of doing this in every one:
public class PickupHolder : MonoBehaviour
{
  public static PickupHolder Instance { get; private set; }

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }
}
