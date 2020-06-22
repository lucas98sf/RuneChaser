using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class Craft : MonoBehaviour
{
  [HideInInspector]
  public GameObject Player;
  public AudioClip Sound;
  public GameObject craftbutton;
  public CraftableItem[] items;
  GameHandler GameHandler;
  Transform buttons;
  Transform numbers;
  GameObject weapon;

  void Start()
  {
    GameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
    weapon = Player.transform.GetChild(2).gameObject;
    numbers = GameObject.Find("GameHandler/Canvas/Inventory/Numbers").transform;
    buttons = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts").transform;
    foreach (CraftableItem item in items)
    {
      GameObject button = Instantiate(craftbutton);
      button.transform.SetParent(buttons, false);
      button.name = item.prefab.name;
      button.GetComponent<Button>().onClick.AddListener(delegate { CraftOnClick(); });//no prefab do button eu apenas conseguia referenciar o script do prefab do craft, e nele o void start nao era iniciado
      if (item.quantCrafted > 1)
      {
        button.transform.GetChild(3).GetComponent<Text>().text = "x" + item.quantCrafted;
      }
      button.transform.GetChild(2).GetComponent<Image>().sprite = item.prefab.transform.GetComponentInChildren<SpriteRenderer>().sprite;
      button.transform.GetChild(1).GetComponent<Text>().text = Regex.Replace(item.name, "([a-z])([A-Z])", "$1 $2");
      int count = 1;
      for (int i = 0; i < item.NeededItems.Length; i++)
      {
        button.transform.GetChild(1).GetChild(0).GetComponent<Text>().text += "(" + item.NeededItems[i].quant + ") " + item.NeededItems[i].item;
        if (item.NeededItems.Length > 1 && count < item.NeededItems.Length)
        {
          button.transform.GetChild(1).GetChild(0).GetComponent<Text>().text += ", ";
          count++;
        }
      }
      if (item.needWorkbench || item.needFurnace || item.needAnvil)
      {
        button.SetActive(false);
      }
    }
  }
  void FixedUpdate()
  { //controla se os botões de craft são clicaveis (são clicaveis apenas se possivel craftar o item)
    foreach (CraftableItem item in items)
    {
      foreach (Transform button in buttons)
      {
        if (item.name == button.name)
        {
          int count = 0;
          for (int i = 0; i < item.NeededItems.Length; i++)
          {
            if (GameHandler.CheckItems(item.NeededItems[i].quant, item.NeededItems[i].item))
            {
              count++;
            }
          }
          if (count >= item.NeededItems.Length)
          {
            button.GetComponent<Button>().interactable = true;
          }
          else
          {
            button.GetComponent<Button>().interactable = false;
          }
        }
      }
    }
  }

  public GameObject FindClosestWorkBench() //para o item ser colocado em cima da workbench quando craftado
  {
    GameObject[] gos;
    gos = GameObject.FindGameObjectsWithTag("WorkBench");
    GameObject closest = null;
    float distance = Mathf.Infinity;
    Vector3 position = transform.position;
    foreach (GameObject go in gos)
    {
      if (go.transform.parent == null)
      {
        Vector3 diff = go.transform.position - position;
        float curDistance = diff.sqrMagnitude;
        if (curDistance < distance)
        {
          closest = go;
          distance = curDistance;
        }
      }
    }
    return closest;
  }

  public GameObject FindClosestFurnace()
  {
    GameObject[] gos;
    gos = GameObject.FindGameObjectsWithTag("Furnace");
    GameObject closest = null;
    float distance = Mathf.Infinity;
    Vector3 position = transform.position;
    foreach (GameObject go in gos)
    {
      if (go.transform.parent == null)
      {
        Vector3 diff = go.transform.position - position;
        float curDistance = diff.sqrMagnitude;
        if (curDistance < distance)
        {
          closest = go;
          distance = curDistance;
        }
      }
    }
    return closest;
  }

  public GameObject FindClosestAnvil()
  {
    GameObject[] gos;
    gos = GameObject.FindGameObjectsWithTag("Anvil");
    GameObject closest = null;
    float distance = Mathf.Infinity;
    Vector3 position = transform.position;
    foreach (GameObject go in gos)
    {
      if (go.transform.parent == null)
      {
        Vector3 diff = go.transform.position - position;
        float curDistance = diff.sqrMagnitude;
        if (curDistance < distance)
        {
          closest = go;
          distance = curDistance;
        }
      }
    }
    return closest;
  }

  public void CraftOnClick() //crafta o item ao pressionar o botão, caso o jogador tenha os itens
  {
    foreach (CraftableItem item in items)
    {
      if (EventSystem.current.currentSelectedGameObject.name == item.name)
      {
        weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
        weapon.tag = "Untagged";
        for (int i = 0; i < item.quantCrafted; i++)
        {
          if (!item.needWorkbench && !item.needFurnace && !item.needAnvil)
          {
            GameHandler.PutInSlot(item.prefab, Sound);
          }
          else
          {
            if (item.needWorkbench)
            {
              Instantiate(item.prefab, FindClosestWorkBench().transform.position + item.position, transform.rotation);
            }
            if (item.needFurnace)
            {
              Instantiate(item.prefab, FindClosestFurnace().transform.position + item.position, transform.rotation);
            }
            if (item.needAnvil)
            {
              Instantiate(item.prefab, FindClosestAnvil().transform.position + item.position, transform.rotation);
            }
          }
        }
        for (int i = 0; i < item.NeededItems.Length; i++)
        {
          GameHandler.DestroyItem(item.NeededItems[i].item, item.NeededItems[i].quant);
        }
        foreach (Transform button in buttons)
        {
          if (item.name == button.name)
          {
            button.GetComponent<Button>().interactable = false;
            return;
          }
        }
      }
    }
  }
}