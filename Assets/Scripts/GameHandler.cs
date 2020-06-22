using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
public class GameHandler : MonoBehaviour
{
  [HideInInspector]
  public Camera cam;
  public AudioClip Pick;
  public static float timer;
  public static float gametime;
  public GameObject PlayerPrefab;
  public Altar Altar;
  public static GameObject Player;
  private bool activeCraft = false;
  private bool activeStats = false;
  private string SelectedObjectMessage;
  public GUIStyle SelectedObjectStyle;
  public static GameObject inventory;
  public GameObject Inventory1;
  public GameObject Inventory2;
  bool message;
  string messageText;
  public GUIStyle messageStyle;
  public Texture2D backg;
  private Vector2 worldPoint;
  Transform slots;
  public static bool Drag = false;
  private Transform SlotClicked;
  private Transform SlotUnderMouse;
  private bool StatUnderMouse;
  //public bool FullInventory = false;
  public CameraController CameraController;
  public PlayerHealthBar PlayerHealthBar;
  public DashCD DashCD;
  public XPBar XPBar;
  public Stats Stats;
  public Arrow Arrow;
  public Bomb Bomb;
  public Craft Craft;
  public static GameObject menu;
  public GameObject InvButton;
  public GameObject CftButton;
  public GameObject StsButton;
  public GameObject Compass;
  public static string timestring;
  public TextMeshProUGUI TimeCounter;
  public static AudioSource Audio;

  // public int treesQuant;
  // public int ironveinsQuant;
  // public int goldveinsQuant;

  void Awake()
  { //o player é instanciado aqui
    Time.timeScale = 1;
    Player = Instantiate(PlayerPrefab);
    Player.transform.SetSiblingIndex(0);
    CameraController.target = Player.transform;
    PlayerHealthBar.Player = Player;
    XPBar.Player = Player;
    DashCD.Player = Player;
    Stats.Player = Player;
    Arrow.Player = Player;
    Bomb.Player = Player;
    Craft.Player = Player;
    Player.GetComponent<PlayerController>().PlayerHealthBar = PlayerHealthBar;
    inventory = Inventory1;
    slots = inventory.transform.Find("Slots").transform;
    cam = Camera.main;
    gametime = 0;
    timer = 0;
    Audio = gameObject.GetComponent<AudioSource>();
    Audio.volume = menu.GetComponent<MainMenu>().FXVolume.value;
  }
  void FixedUpdate()
  {
    menu.GetComponent<MainMenu>().Music.volume = menu.GetComponent<MainMenu>().MusicVolume.value;
    Audio.volume = menu.GetComponent<MainMenu>().FXVolume.value;
    gametime += Time.fixedDeltaTime;
    if (gametime >= 180)
    {
      timer += Time.fixedDeltaTime; //escalonamento com o tempo depois de 3 minutos
    }
    Rigidbody2D rb = Compass.GetComponent<Rigidbody2D>();
    Vector3 pos = rb.position;
    Vector2 lookDir = Altar.transform.position - pos;
    float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 270;
    rb.rotation = angle;
    timestring = string.Format("{0:0}:{1:00}", Mathf.FloorToInt(gametime / 60F), Mathf.FloorToInt(GameHandler.gametime - Mathf.FloorToInt(gametime / 60F) * 60));
    TimeCounter.text = timestring;
  }
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Q))
    {
      ChangeInv();
    }

    if (inventory == null)
    {
      inventory = Inventory1;
    }

    slots = inventory.transform.Find("Slots").transform;

    if (EventSystem.current.currentSelectedGameObject != null && (EventSystem.current.currentSelectedGameObject.name == "Scrollbar" || EventSystem.current.currentSelectedGameObject.name == "Inv" || EventSystem.current.currentSelectedGameObject.name == "Cft" || EventSystem.current.currentSelectedGameObject.name == "Sts"))
    {
      EventSystem.current.SetSelectedGameObject(null);
    }

    foreach (Transform child in slots)
    { //mostra o numero de itens caso houver mais que 1
      inventory.transform.Find("Numbers").transform.Find(child.name).GetComponent<Text>().text = "" + child.gameObject.GetComponent<InventorySlot>().ItemCount + "";
      if (child.gameObject.GetComponent<InventorySlot>().ItemCount < 2)
      {
        inventory.transform.Find("Numbers").transform.Find(child.name).GetComponent<Text>().text = "";
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
      OpenCraft();
    }

    if (Input.GetKeyDown(KeyCode.C))
    { //tela de atributos
      OpenStats();
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
          StatUnderMouse = true;
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
          StatUnderMouse = false;
          SelectedObjectMessage = null;
          SelectedObjectStyle.normal.background = null;
        }
      }
    }
    else
    {
      StatUnderMouse = false;
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
      RectTransformUtility.ScreenPointToLocalPointInRectangle(GameObject.Find("Canvas").transform as RectTransform, Input.mousePosition, cam, out pos);
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
      if (Input.GetMouseButtonUp(0) && SlotUnderMouse != null && SlotUnderMouse.childCount > 0 && SlotUnderMouse != SlotClicked && !SlotUnderMouse.GetChild(0).CompareTag(SlotClicked.GetChild(0).tag))
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
      if (Input.GetMouseButtonUp(0) && SlotUnderMouse != null && SlotUnderMouse.childCount > 0 && SlotUnderMouse != SlotClicked && SlotUnderMouse.GetChild(0).CompareTag(SlotClicked.GetChild(0).tag))
      {
        SlotUnderMouse.GetComponent<InventorySlot>().ItemCount += SlotClicked.GetComponent<InventorySlot>().ItemCount;
        Destroy(SlotClicked.GetChild(0).gameObject);
        SlotClicked.GetComponent<InventorySlot>().ItemCount = 0;
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
      GUI.Box(new Rect(Input.mousePosition.x + 30, Screen.height - Input.mousePosition.y, 90, 25), SelectedObjectMessage, SelectedObjectStyle);
    }
    else
    {
      if (StatUnderMouse)
      {
        GUI.Box(new Rect(Input.mousePosition.x + 30, Screen.height - Input.mousePosition.y, 150, 80), SelectedObjectMessage, SelectedObjectStyle);
      }
    }
    if (message)
    {
      GUI.Box(new Rect(Input.mousePosition.x + 30, Screen.height - Input.mousePosition.y, 100, 20), messageText, messageStyle);
    }
  }

  public IEnumerator Message(string text)
  {
    if (!message)
    {
      message = true;
      messageText = text;
      yield return new WaitForSeconds(2);
      message = false;
      yield break;
    }
  }

  public static Transform FindSlot(GameObject go)
  { //acha o slot disponivel, caso já tenha o item apenas aumenta o count
    foreach (Transform child in inventory.transform.Find("Slots").transform)
    {
      if (child.transform.childCount > 0 && child.GetChild(0).GetComponent<Image>().sprite == go.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite)
      {
        child.GetComponent<InventorySlot>().ItemCount++;
        return null;
      }
    }
    GameObject NextSlotGO = new GameObject();
    Transform NextSlot = NextSlotGO.transform;
    foreach (Transform child in inventory.transform.Find("Slots").transform)
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

  public static void PutInSlot(GameObject go, AudioClip sound)
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
        NewItem.GetComponent<RectTransform>().localPosition = new Vector3(0, 5, 0);
      }
      else
      {
        NewItem.GetComponent<RectTransform>().localPosition = Vector3.zero;
      }
      NewItem.GetComponent<Image>().sprite = go.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
      Audio.PlayOneShot(sound);
      return;
    }
    Audio.PlayOneShot(sound);
    return;
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

  public void DestroyItem(string tag, int quant)
  { //destroi os itens usados no craft
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

  public void Back()
  {
    if (GameObject.Find("GameHandler/Canvas/GameOver").activeInHierarchy)
    {
      GameObject.Find("GameHandler/Canvas/GameOver").SetActive(false);
    }
    if (GameObject.Find("GameHandler/Canvas/GameWin").activeInHierarchy)
    {
      GameObject.Find("GameHandler/Canvas/GameWin").SetActive(false);
    }
    Destroy(Audio);
    SceneManager.LoadSceneAsync("MainMenu");
  }

  public void ChangeInv()
  {
    if (Inventory1.activeInHierarchy)
    {
      PlayerController.ClearSelected();
      PlayerController.SelectedSlot = null;
      Inventory1.SetActive(false);
      Inventory2.SetActive(true);
      inventory = Inventory2;
      InvButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "2";
    }
    else
    {
      PlayerController.ClearSelected();
      PlayerController.SelectedSlot = null;
      Inventory2.SetActive(false);
      Inventory1.SetActive(true);
      inventory = Inventory1;
      InvButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
    }
  }
  public void OpenCraft()
  {
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
  public void OpenStats()
  {
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
}