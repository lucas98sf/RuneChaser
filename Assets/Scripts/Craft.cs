using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Craft : MonoBehaviour
{
  public GameObject Player;
  GameObject weapon;
  public GameObject StoneAxePrefab;
  public GameObject StonePickaxePrefab;
  public GameObject WorkBenchPrefab;
  public GameObject StickPrefab;
  public GameObject WoodenSwordPrefab;
  public GameObject FurnacePrefab;
  public GameObject AnvilPrefab;
  public GameObject IronSwordPrefab;
  public GameObject GoldenSwordPrefab;
  public GameObject IronBarPrefab;
  public GameObject GoldBarPrefab;
  Transform slots;
  Transform numbers;
  GameObject StoneAxe;
  GameObject StonePickaxe;
  GameObject WorkBench;
  GameObject Stick;
  GameObject WoodenSword;
  GameObject Furnace;
  GameObject Anvil;
  GameObject IronSword;
  GameObject GoldenSword;
  GameObject IronBar;
  GameObject GoldBar;

  void Start()
  {
    weapon = Player.transform.GetChild(2).gameObject;
    numbers = GameObject.Find("GameHandler/Canvas/Inventory/Numbers").transform;
    slots = GameObject.Find("GameHandler/Canvas/Inventory/Slots").transform;
    StoneAxe = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/StoneAxe");
    StonePickaxe = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/StonePickaxe");
    WorkBench = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/WorkBench");
    Stick = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/Stick");
    WoodenSword = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/WoodenSword");
    Furnace = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/Furnace");
    IronBar = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/IronBar");
    GoldBar = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/GoldBar");
    Anvil = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/Anvil");
    IronSword = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/IronSword");
    GoldenSword = GameObject.Find("GameHandler/Canvas/Craft/Scroll/Crafts/GoldenSword");
  }
  void FixedUpdate()
  { //controla se os botões de craft são clicaveis (são clicaveis apenas se possivel craftar o item)
    //StoneAxe
    if (CheckItems(3, "Rock") && CheckItems(1, "Stick"))
    {
      StoneAxe.GetComponent<Button>().interactable = true;
    }
    else { StoneAxe.GetComponent<Button>().interactable = false; }
    //StonePickaxe
    if (CheckItems(4, "Rock") && CheckItems(1, "Stick"))
    {
      StonePickaxe.GetComponent<Button>().interactable = true;
    }
    else { StonePickaxe.GetComponent<Button>().interactable = false; }
    //WorkBench
    if (CheckItems(4, "Plank"))
    {
      WorkBench.GetComponent<Button>().interactable = true;
    }
    else { WorkBench.GetComponent<Button>().interactable = false; }
    //Stick
    if (CheckItems(1, "Plank"))
    {
      Stick.GetComponent<Button>().interactable = true;
    }
    else { Stick.GetComponent<Button>().interactable = false; }
    //WoodenSword
    if (CheckItems(3, "Plank") && CheckItems(1, "Stick"))
    {
      WoodenSword.GetComponent<Button>().interactable = true;
    }
    else { WoodenSword.GetComponent<Button>().interactable = false; }
    //Furnace
    if (CheckItems(10, "Rock"))
    {
      Furnace.GetComponent<Button>().interactable = true;
    }
    else { Furnace.GetComponent<Button>().interactable = false; }
    //IronBar
    if (CheckItems(3, "IronOre") && CheckItems(1, "Log"))
    {
      IronBar.GetComponent<Button>().interactable = true;
    }
    else { IronBar.GetComponent<Button>().interactable = false; }
    //GoldBar
    if (CheckItems(3, "GoldOre") && CheckItems(1, "Log"))
    {
      GoldBar.GetComponent<Button>().interactable = true;
    }
    else { GoldBar.GetComponent<Button>().interactable = false; }
    //Anvil
    if (CheckItems(6, "IronBar"))
    {
      Anvil.GetComponent<Button>().interactable = true;
    }
    else { Anvil.GetComponent<Button>().interactable = false; }
    //IronSword
    if (CheckItems(5, "IronBar") && CheckItems(1, "WoodenSword"))
    {
      IronSword.GetComponent<Button>().interactable = true;
    }
    else { IronSword.GetComponent<Button>().interactable = false; }
    //GoldenSword
    if (CheckItems(1, "IronSword") && CheckItems(7, "GoldBar"))
    {
      GoldenSword.GetComponent<Button>().interactable = true;
    }
    else { GoldenSword.GetComponent<Button>().interactable = false; }
  }

  public bool CheckItems(int quant, string tag)
  { //faz a checagem da quantidade de itens
    foreach (Transform child in slots)
    {
      if (child.childCount > 0 && child.GetChild(0).CompareTag(tag))
      {
        if (child.GetComponent<InventorySlot>().ItemCount >= quant)
        {
          return true;
        }
      }
    }
    return false;
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
  {//StoneAxe
    if (EventSystem.current.currentSelectedGameObject.name == "StoneAxe")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      GameHandler.PutInSlot(StoneAxePrefab);
      DestroyItem("Rock", 3);
      DestroyItem("Stick", 1);
      StoneAxe.GetComponent<Button>().interactable = false;
      return;
    }
    //StonePickaxe
    if (EventSystem.current.currentSelectedGameObject.name == "StonePickaxe")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      GameHandler.PutInSlot(StonePickaxePrefab);
      DestroyItem("Rock", 4);
      DestroyItem("Stick", 1);
      StonePickaxe.GetComponent<Button>().interactable = false;
      return;
    }
    //WorkBench
    if (EventSystem.current.currentSelectedGameObject.name == "WorkBench")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      GameHandler.PutInSlot(WorkBenchPrefab);
      DestroyItem("Plank", 4);
      WorkBench.GetComponent<Button>().interactable = false;
      return;
    }
    //Stick
    if (EventSystem.current.currentSelectedGameObject.name == "Stick")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      Instantiate(StickPrefab, FindClosestWorkBench().transform.position + new Vector3(-0.1f, 0.2f, 0), transform.rotation);
      Instantiate(StickPrefab, FindClosestWorkBench().transform.position + new Vector3(0.1f, 0.2f, 0), transform.rotation);
      DestroyItem("Plank", 1);
      Stick.GetComponent<Button>().interactable = false;
      return;
    }
    //WoodenSword
    if (EventSystem.current.currentSelectedGameObject.name == "WoodenSword")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      Instantiate(WoodenSwordPrefab, FindClosestWorkBench().transform.position + new Vector3(0, 0.2f, 0), transform.rotation);
      DestroyItem("Plank", 3);
      DestroyItem("Stick", 1);
      WoodenSword.GetComponent<Button>().interactable = false;
      return;
    }
    //Furnace
    if (EventSystem.current.currentSelectedGameObject.name == "Furnace")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      Instantiate(FurnacePrefab, FindClosestWorkBench().transform.position + new Vector3(0, 0.3f, 0), transform.rotation);
      DestroyItem("Rock", 10);
      Furnace.GetComponent<Button>().interactable = false;
      return;
    }
    //IronBar
    if (EventSystem.current.currentSelectedGameObject.name == "IronBar")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      Instantiate(IronBarPrefab, FindClosestFurnace().transform.position + new Vector3(0, -0.3f, 0), transform.rotation);
      DestroyItem("IronOre", 3);
      DestroyItem("Log", 1);
      IronBar.GetComponent<Button>().interactable = false;
      return;
    }
    //GoldBar
    if (EventSystem.current.currentSelectedGameObject.name == "GoldBar")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      Instantiate(GoldBarPrefab, FindClosestFurnace().transform.position + new Vector3(0, -0.3f, 0), transform.rotation);
      DestroyItem("GoldOre", 3);
      DestroyItem("Log", 1);
      GoldBar.GetComponent<Button>().interactable = false;
      return;
    }
    //Anvil
    if (EventSystem.current.currentSelectedGameObject.name == "Anvil")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      Instantiate(AnvilPrefab, FindClosestWorkBench().transform.position + new Vector3(0, 0.3f, 0), transform.rotation);
      DestroyItem("IronBar", 6);
      Anvil.GetComponent<Button>().interactable = false;
      return;
    }
    //IronSword
    if (EventSystem.current.currentSelectedGameObject.name == "IronSword")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      Instantiate(IronSwordPrefab, FindClosestAnvil().transform.position + new Vector3(0, 0.2f, 0), transform.rotation);
      DestroyItem("IronBar", 5);
      DestroyItem("WoodenSword", 1);
      IronSword.GetComponent<Button>().interactable = false;
      return;
    }
    //GoldenSword
    if (EventSystem.current.currentSelectedGameObject.name == "GoldenSword")
    {
      weapon.transform.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
      Instantiate(GoldenSwordPrefab, FindClosestAnvil().transform.position + new Vector3(0, 0.2f, 0), transform.rotation);
      DestroyItem("IronSword", 1);
      DestroyItem("GoldBar", 7);
      GoldenSword.GetComponent<Button>().interactable = false;
      return;
    }
  }
  public void DestroyItem(string tag, int quant)
  { //destrói os itens usados no craft
    foreach (Transform child in slots)
    {
      if (child.transform.childCount > 0 && child.GetChild(0).CompareTag(tag))
      {
        if (child.GetComponent<InventorySlot>().ItemCount >= quant)
        {
          child.GetComponent<InventorySlot>().ItemCount -= quant;
          return;
        }
      }
    }
    return;
  }
}