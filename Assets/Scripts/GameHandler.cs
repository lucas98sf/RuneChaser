using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEditor;
public class GameHandler : MonoBehaviour
{
  public GameObject PlayerPrefab;
  public static GameObject Player;
  private bool activeCraft = false;
  private bool activeStats = false;
  private string SelectedObjectMessage;
  public GUIStyle SelectedObjectStyle;
  public string message;
  bool msg;
  public Texture2D backg;
  private Vector2 worldPoint;
  Transform slots;
  public static bool Drag = false;
  public Transform SlotClicked;
  public Transform SlotUnderMouse;
  public bool FullInventory = false;
  public CameraController CameraController;
  public PlayerHealthBar PlayerHealthBar;
  public XPBar XPBar;
  public DashCD DashCD;
  public Stats Stats;
  public Arrow Arrow;

  // public int treesQuant;
  // public int ironveinsQuant;
  // public int goldveinsQuant;

  void Awake()
  { //o player é instanciado aqui
    Time.timeScale = 1f;
    Player = Instantiate(PlayerPrefab);
    Player.transform.SetSiblingIndex(0);
    CameraController.target = Player.transform;
    PlayerHealthBar.Player = Player;
    XPBar.Player = Player;
    DashCD.Player = Player;
    Stats.Player = Player;
    Arrow.Player = Player;
  }
  void Start()
  {
    slots = GameObject.Find("GameHandler/Canvas/Inventory/Slots").transform;
  }
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    { //menu, ainda sem salvar a scene ao apertar
      SceneManager.LoadScene("Main Menu");
    }

    foreach (Transform child in slots)
    { //mostra o numero de itens caso houver mais que 1
      GameObject.Find("GameHandler/Canvas/Inventory/Numbers/" + child.name).GetComponent<Text>().text = "" + child.gameObject.GetComponent<InventorySlot>().ItemCount + "";
      if (child.gameObject.GetComponent<InventorySlot>().ItemCount < 2)
      {
        GameObject.Find("GameHandler/Canvas/Inventory/Numbers/" + child.name).GetComponent<Text>().text = "";
      }
    }
    //usado apenas para ver a quantidade de objetos gerados

    // GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");  
    // treesQuant = trees.Length;
    // GameObject[] ironveins = GameObject.FindGameObjectsWithTag("IronVein");
    // ironveinsQuant = ironveins.Length;
    // GameObject[] goldveins = GameObject.FindGameObjectsWithTag("GoldVein");
    // goldveinsQuant = goldveins.Length;

    if (Input.GetKeyDown(KeyCode.E))
    { //tela de craft
      if (activeCraft == false)
      {
        GameObject.Find("GameHandler/Canvas/Craft").SetActive(true);
        activeCraft = true;
        return;
      }
      if (activeCraft == true)
      {
        GameObject.Find("GameHandler/Canvas/Craft").SetActive(false);
        activeCraft = false;
        return;
      }
    }

    if (Input.GetKeyDown(KeyCode.C))
    { //tela de atributos
      if (activeStats == false)
      {
        GameObject.Find("GameHandler/Canvas/Stats").SetActive(true);
        activeStats = true;
        return;
      }
      if (activeStats == true)
      {
        GameObject.Find("GameHandler/Canvas/Stats").SetActive(false);
        activeStats = false;
        return;
      }
    }

    if (EventSystem.current.IsPointerOverGameObject())
    { //mostrar nome na messagebox
      List<RaycastResult> results = new List<RaycastResult>();
      var pointer = new PointerEventData(EventSystem.current);
      pointer.position = Input.mousePosition;
      EventSystem.current.RaycastAll(pointer, results);
      foreach (RaycastResult result in results)
      {
        if (result.isValid && result.gameObject.CompareTag("Slot"))
        {
          SlotUnderMouse = result.gameObject.transform;
        }
        else
        {
          SlotUnderMouse = null;
        }
        if (result.isValid && result.gameObject.CompareTag("Slot") && result.gameObject.transform.childCount > 0)
        {
          string GoName = result.gameObject.transform.GetChild(0).name.Replace("(Clone)", "");
          GoName = Regex.Replace(GoName, "([a-z])([A-Z])", "$1 $2");
          SelectedObjectMessage = GoName;
          SelectedObjectStyle.normal.background = backg;
          SelectedObjectStyle.alignment = TextAnchor.MiddleCenter;
          SelectedObjectStyle.padding.left = 0;
          SelectedObjectStyle.padding.top = 0;
          SelectedObjectStyle.padding.right = 0;
          if (Input.GetMouseButtonDown(0))
          {
            Drag = true;
            SlotClicked = result.gameObject.transform;
          }
        }
        if (result.isValid && result.gameObject.CompareTag("Stat"))
        { //informações dos stats
          if (result.gameObject.name == "Points")
          {
            SelectedObjectMessage = "Earn 1 for each level +1 bonus point for each 5 levels";
            SelectedObjectStyle.normal.background = backg;
            SelectedObjectStyle.alignment = TextAnchor.UpperLeft;
            SelectedObjectStyle.padding.left = 10;
            SelectedObjectStyle.padding.top = 10;
            SelectedObjectStyle.padding.right = 10;
          }
          if (result.gameObject.name == "Strength")
          {
            SelectedObjectMessage = "Greatly increases sword damage and slightly increases bow damage";
            SelectedObjectStyle.normal.background = backg;
            SelectedObjectStyle.alignment = TextAnchor.UpperLeft;
            SelectedObjectStyle.padding.left = 10;
            SelectedObjectStyle.padding.top = 10;
            SelectedObjectStyle.padding.right = 10;
          }
          if (result.gameObject.name == "Agility")
          {
            SelectedObjectMessage = "Reduces delay between attacks";
            SelectedObjectStyle.normal.background = backg;
            SelectedObjectStyle.alignment = TextAnchor.UpperLeft;
            SelectedObjectStyle.padding.left = 10;
            SelectedObjectStyle.padding.top = 10;
            SelectedObjectStyle.padding.right = 10;
          }
          if (result.gameObject.name == "Toughness")
          {
            SelectedObjectMessage = "Increases resistance to damage";
            SelectedObjectStyle.normal.background = backg;
            SelectedObjectStyle.alignment = TextAnchor.UpperLeft;
            SelectedObjectStyle.padding.left = 10;
            SelectedObjectStyle.padding.top = 10;
            SelectedObjectStyle.padding.right = 10;
          }
          if (result.gameObject.name == "Vigor")
          {
            SelectedObjectMessage = "Increases health and health regeneration";
            SelectedObjectStyle.normal.background = backg;
            SelectedObjectStyle.alignment = TextAnchor.UpperLeft;
            SelectedObjectStyle.padding.left = 10;
            SelectedObjectStyle.padding.top = 10;
            SelectedObjectStyle.padding.right = 10;
          }
          if (result.gameObject.name == "Dexterity")
          {
            SelectedObjectMessage = "Greatly increases bow damage and slightly increases critical chance and damage";
            SelectedObjectStyle.normal.background = backg;
            SelectedObjectStyle.alignment = TextAnchor.UpperLeft;
            SelectedObjectStyle.padding.left = 10;
            SelectedObjectStyle.padding.top = 10;
            SelectedObjectStyle.padding.right = 10;
          }
          if (result.gameObject.name == "Swiftness")
          {
            SelectedObjectMessage = "Increases movement speed and reduces dash cooldown and range";
            SelectedObjectStyle.normal.background = backg;
            SelectedObjectStyle.alignment = TextAnchor.UpperLeft;
            SelectedObjectStyle.padding.left = 10;
            SelectedObjectStyle.padding.top = 10;
            SelectedObjectStyle.padding.right = 10;
          }
        }
        if (result.isValid == false)
        {
          SelectedObjectMessage = null;
          SelectedObjectStyle.normal.background = null;
        }
      }
    }
    else
    {
      SelectedObjectMessage = null;
      SelectedObjectStyle.normal.background = null;
    }
    // else{
    // worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    // RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
    //     if (hit.collider != null)
    //     {
    //         string GoName = hit.collider.name.Replace("(Clone)", "");
    //         GoName = Regex.Replace(GoName, "([a-z])([A-Z])", "$1 $2");
    //         SelectedObjectMessage = GoName;
    //         SelectedObjectStyle.normal.background = backg;
    //     }else{
    //         SelectedObjectMessage = null;
    //         SelectedObjectStyle.normal.background = null;
    //     }
    // }

    if (Drag)
    { //arrastar itens no inventário
      Transform Item = SlotClicked.GetChild(0);
      Vector2 pos;
      Item.GetComponent<Image>().raycastTarget = false;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").transform as RectTransform, Input.mousePosition, Camera.main, out pos);
      Item.transform.position = GameObject.Find("Canvas").transform.TransformPoint(pos);
      if (Input.GetMouseButtonUp(0) && SlotUnderMouse != null && SlotUnderMouse.childCount == 0)
      {
        int amount = SlotClicked.GetComponent<InventorySlot>().ItemCount;
        SlotUnderMouse.GetComponent<InventorySlot>().ItemCount = amount;
        SlotClicked.GetComponent<InventorySlot>().ItemCount = 0;
        Item.SetParent(SlotUnderMouse);
        Item.transform.localPosition = Vector3.zero;
        Item.GetComponent<Image>().raycastTarget = true;
        Drag = false;
        return;
      }
      if (Input.GetMouseButtonUp(0) && SlotUnderMouse.childCount > 0 && SlotUnderMouse != null && SlotUnderMouse != SlotClicked)
      {
        int amount = SlotUnderMouse.GetComponent<InventorySlot>().ItemCount;
        SlotUnderMouse.GetComponent<InventorySlot>().ItemCount = SlotClicked.GetComponent<InventorySlot>().ItemCount;
        SlotClicked.GetComponent<InventorySlot>().ItemCount = amount;
        SlotUnderMouse.GetChild(0).SetParent(SlotClicked);
        Item.SetParent(SlotUnderMouse);
        SlotClicked.GetChild(0).localPosition = Vector3.zero;
        SlotUnderMouse.GetChild(0).localPosition = Vector3.zero;
        Item.GetComponent<Image>().raycastTarget = true;
        Drag = false;
        return;
      }
      if (Input.GetMouseButtonUp(0) && SlotUnderMouse == SlotClicked)
      {
        Item.transform.localPosition = Vector3.zero;
        Item.GetComponent<Image>().raycastTarget = true;
        Drag = false;
        return;
      }
    }
  }

  void OnGUI()
  {
    if (SlotUnderMouse != null)
    {
      GUI.Box(new Rect(Input.mousePosition.x + 30f, Screen.height - Input.mousePosition.y, 90f, 25f), SelectedObjectMessage, SelectedObjectStyle);
    }
    else
    {
      GUI.Box(new Rect(Input.mousePosition.x + 30f, Screen.height - Input.mousePosition.y, 150f, 80f), SelectedObjectMessage, SelectedObjectStyle);
    }
  }

  public static Transform FindSlot(GameObject go)
  { //acha o slot disponivel, caso já tenha o item apenas aumenta o count
    foreach (Transform child in GameObject.Find("GameHandler/Canvas/Inventory/Slots").transform)
    {
      if (child.transform.childCount > 0 && child.GetChild(0).GetComponent<Image>().sprite == go.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite)
      {
        child.GetComponent<InventorySlot>().ItemCount++;
        return null;
      }
    }
    GameObject NextSlotGO = new GameObject();
    Transform NextSlot = NextSlotGO.transform;
    foreach (Transform child in GameObject.Find("GameHandler/Canvas/Inventory/Slots").transform)
    {
      if (child.transform.childCount == 0)
      {
        NextSlot = child.GetComponent<Transform>();
        Destroy(NextSlotGO);
        return NextSlot as Transform;
      }
    }
    Destroy(NextSlotGO);
    return null;
  }

  public static void PutInSlot(GameObject go)
  {  //cria um novo slot e põe o item
    if (FindSlot(go) != null)
    {
      FindSlot(go).GetComponent<InventorySlot>().ItemCount++;
      GameObject NewItem = new GameObject();
      NewItem.transform.position = FindSlot(go).position;
      NewItem.transform.parent = FindSlot(go);
      NewItem.tag = go.tag;
      NewItem.name = go.name;
      NewItem.AddComponent<Image>();
      NewItem.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
      if (go.CompareTag("Bow"))
      { //para o arco não ficar muito em baixo no slot
        NewItem.GetComponent<RectTransform>().localPosition = new Vector3(0f, 5f, 0f);
      }
      else
      {
        NewItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
      }
      NewItem.GetComponent<Image>().sprite = go.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
      return;
    }
    return;
  }

  public void GameOverBack()
  {
    GameObject.Find("GameHandler/Canvas/GameOver").SetActive(false);
    SceneManager.LoadScene("Main Menu");
  }
}