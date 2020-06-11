using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
  public int ItemCount;
  public Image SlotImage;
  void Awake()
  {
    ItemCount = 0;
    foreach (Transform child in GameObject.Find("Canvas/Inventory/SlotImages").transform)
    { //relaciona os slots e seus backgrounds (separei por problemas com o drag n drop)
      if (child.name == gameObject.name)
      {
        SlotImage = child.GetComponent<Image>();
      }
    }
  }
  void FixedUpdate()
  {
    if (transform.childCount > 0 && ItemCount == 0)
    { //destroi o item caso sua quantidade seja 0
      Destroy(transform.GetChild(0).gameObject);
    }
  }
}