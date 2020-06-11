using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickItem : MonoBehaviour
{
  private bool istriggered = false;

  void OnTriggerStay2D(Collider2D other)
  { // pegar itens do chão
    if (other.CompareTag("Player"))
    {
      if (istriggered == false)
      {
        if (Input.GetKeyDown(KeyCode.F))
        {
          istriggered = true;
          int fullSlots = 0;
          foreach (Transform child in GameObject.Find("GameHandler/Canvas/Inventory/Slots").transform)
          {
            if (child.transform.childCount > 0 && !child.transform.GetChild(0).CompareTag(gameObject.tag))
            {
              fullSlots++;
            }
          }
          if (fullSlots == 9)
          {
            istriggered = false;
            return;
          }
          else
          {
            GameHandler.PutInSlot(gameObject);
            Destroy(gameObject);
            istriggered = false;
          }
        }
      }
    }
  }
}