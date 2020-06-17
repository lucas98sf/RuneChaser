using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour
{
  //permite o player craftar quando próximo (dentro do collider)
  public CraftableItem[] items;
  Transform buttons;
  void Start()
  {
    buttons = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts").transform;
    items = GameObject.Find("GameHandler/Canvas/Craft").GetComponent<Craft>().items;
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      if (gameObject.transform.parent.CompareTag("WorkBench"))
      {
        foreach (CraftableItem item in items)
        {
          if (item.needWorkbench)
          {
            foreach (Transform button in buttons)
            {
              if (item.name == button.name)
              {
                button.gameObject.SetActive(true);
              }
            }
          }
        }
      }
      // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/Stick").SetActive(true);
      // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/WoodenSword").SetActive(true);
      // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/Furnace").SetActive(true);
      // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/Anvil").SetActive(true);
    }
    if (gameObject.transform.parent.CompareTag("Furnace"))
    {
      foreach (CraftableItem item in items)
      {
        if (item.needFurnace)
        {
          foreach (Transform button in buttons)
          {
            if (item.name == button.name)
            {
              button.gameObject.SetActive(true);
            }
          }
        }
      }
      // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/IronBar").SetActive(true);
      // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/GoldBar").SetActive(true);
    }
    if (gameObject.transform.parent.CompareTag("Anvil"))
    {
      foreach (CraftableItem item in items)
      {
        if (item.needAnvil)
        {
          foreach (Transform button in buttons)
          {
            if (item.name == button.name)
            {
              button.gameObject.SetActive(true);
            }
          }
        }
      }
      // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/IronSword").SetActive(true);
      // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/GoldenSword").SetActive(true);
    }
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (other.CompareTag("Player"))
    {
      if (gameObject.transform.parent.CompareTag("WorkBench"))
      {
        foreach (CraftableItem item in items)
        {
          if (item.needWorkbench)
          {
            foreach (Transform button in buttons)
            {
              if (item.name == button.name)
              {
                button.gameObject.SetActive(false);
              }
            }
          }
        }
        // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/Stick").SetActive(false);
        // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/WoodenSword").SetActive(false);
        // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/Furnace").SetActive(false);
        // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/Anvil").SetActive(false);
      }
      if (gameObject.transform.parent.CompareTag("Furnace"))
      {
        foreach (CraftableItem item in items)
        {
          if (item.needFurnace)
          {
            foreach (Transform button in buttons)
            {
              if (item.name == button.name)
              {
                button.gameObject.SetActive(false);
              }
            }
          }
        }
        // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/IronBar").SetActive(false);
        // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/GoldBar").SetActive(false);
      }
      if (gameObject.transform.parent.CompareTag("Anvil"))
      {
        foreach (CraftableItem item in items)
        {
          if (item.needAnvil)
          {
            foreach (Transform button in buttons)
            {
              if (item.name == button.name)
              {
                button.gameObject.SetActive(false);
              }
            }
          }
        }
      }
      // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/IronSword").SetActive(false);
      // GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/GoldenSword").SetActive(false);
    }
  }
}