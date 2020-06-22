using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerController : MonoBehaviour
{
  private Camera cam;
  GameHandler GameHandler;
  public AudioClip Swing;
  public AudioClip Bow;
  public AudioClip Success;
  public AudioClip Error;
  public AudioClip Drop;
  public AudioClip DashSound;
  [Header("Player Components")]
  [HideInInspector]
  public PlayerHealthBar PlayerHealthBar;
  [HideInInspector]
  public static GameObject inventory;
  Vector2 lookDir;
  public Rigidbody2D rb;
  public Transform attackPoint;
  public GameObject Dash;
  Vector2 movement;
  Vector2 mousePos;
  GameObject weapon;
  GameObject righth;
  GameObject lefth;
  Transform slots;
  [HideInInspector]
  public static GameObject SelectedSlot;
  Animation anim;
  public float FishTime;
  public FishText FishText;
  [Header("Prefabs")]
  public GameObject BlueRune;
  public List<GameObject> FishPrefabs;
  public GameObject[] Prefabs;
  private GameObject Prefab;
  public GameObject Arrow;
  public GameObject Bomb;
  [Header("Targets")]
  public LayerMask AxeTargetLayers;
  public LayerMask PickaxeTargetLayers;
  public LayerMask EnemyLayers;
  public LayerMask LakeLayers;
  public LayerMask AltarLayer;
  public LayerMask CrystalLayer;
  [Header("Player Stats")]
  public float baseMoveSpeed;
  public float attackRange;
  public float arrowSpeed;
  public float bombSpeed;
  public float baseBowDamage;
  public float baseSwordDamage;
  public float basedashCD;
  public float basehealth;
  public float baseregen = 5;
  public int strength;
  public int agility;
  public int vigor;
  public int dexterity;
  public int swiftness;
  [Header("In Game")]
  public float MoveSpeed;
  public int BowDamage;
  public int SwordDamage;
  public float attackspeed = 0;
  public float dashCD;
  public float dashrange = 0;
  public float health;
  public float regen = 5;
  public float regenquant = 1;
  public int level = 1;
  public float points = 1;
  public float baseexp;
  public float exp;
  //public float armor = 0;
  //public float Resistance;
  public float CritChance;
  public float CritMult;
  private bool canAxePick = true;
  private bool canDrop = true;
  private bool canScroll = true;
  private bool canAttack = true;
  private bool canBow = true;
  private bool canBomb = true;
  private bool Fishing;
  private bool hit;
  [HideInInspector] public bool canDash = true;
  [HideInInspector] public bool canDashWall = true;
  private int quant = 0;

  void Start()
  {
    GameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
    inventory = GameHandler.inventory;
    cam = Camera.main;
    anim = GetComponent<Animation>();
    weapon = this.gameObject.transform.GetChild(2).gameObject;
    righth = this.gameObject.transform.GetChild(1).GetChild(1).gameObject;
    lefth = this.gameObject.transform.GetChild(1).GetChild(0).gameObject;
    slots = inventory.transform.Find("Slots").transform;
    baseexp = Mathf.RoundToInt(level * 1.2f * 16);
  }

  IEnumerator AttackDelay(Collider2D target, float damage)
  { //delay no ataque
    canAttack = false;
    yield return new WaitForSeconds(0.25f * (1 / attackspeed));
    if (target != null)
    {
      if (Random.value <= CritChance)
      {
        target.gameObject.GetComponent<HealthBar>().TakeDamage(damage * CritMult, true);
      }
      else
      {
        target.gameObject.GetComponent<HealthBar>().TakeDamage(damage, false);
      }
      target.GetComponent<Rigidbody2D>().AddForce((target.transform.position - gameObject.transform.position).normalized * (damage * 5));
    }
    yield return new WaitForSeconds(0.4f * (1 / attackspeed));
    canAttack = true;
  }

  IEnumerator AxePickDelay(Collider2D target)
  {
    canAxePick = false;
    yield return new WaitForSeconds(0.25f * (1 / attackspeed));
    if (target != null)
    {
      target.gameObject.GetComponent<Animation>().Play();
    }
    yield return new WaitForSeconds(0.4f * (1 / attackspeed));
    if (target != null)
    {
      if (weapon.CompareTag("VilePickAxe"))
      {
        target.GetComponent<HealthBar>().TakeDamage(2, false);
      }
      else
      {
        target.GetComponent<HealthBar>().TakeDamage(1, false);
      }
    }
    canAxePick = true;
  }

  IEnumerator ShootBow()
  {
    canBow = false;
    yield return new WaitForSeconds(0.77f * (1 / attackspeed));
    GameHandler.Audio.PlayOneShot(Bow);
    GameObject arrow = Instantiate(Arrow, attackPoint.position, attackPoint.rotation);
    Rigidbody2D arrowrb = arrow.GetComponent<Rigidbody2D>();
    GameHandler.DestroyItem("Arrow", 1);
    arrowrb.AddForce(attackPoint.up * arrowSpeed, ForceMode2D.Impulse);
    yield return new WaitForSeconds(0.1f * (1 / attackspeed));
    canBow = true;
  }

  IEnumerator ThrowBomb()
  {
    canBomb = false;
    yield return new WaitForSeconds(0.3f * (1 / attackspeed));
    GameObject bomb = Instantiate(Bomb, attackPoint.position, attackPoint.rotation);
    Rigidbody2D bombrb = bomb.GetComponent<Rigidbody2D>();
    GameHandler.DestroyItem("PufferFish", 1);
    bombrb.AddForce(attackPoint.up * bombSpeed, ForceMode2D.Impulse);
    yield return new WaitForSeconds(0.57f * (1 / attackspeed));
    canBomb = true;
  }

  IEnumerator DashDelay()
  {
    canDash = false;
    yield return new WaitForSeconds(dashCD);
    canDash = true;
  }
  private void HandleMovement()
  {
    float moveX = 0f;
    float moveY = 0f;
    if (Input.GetKey(KeyCode.W))
    {
      moveY = +1f;
    }
    if (Input.GetKey(KeyCode.A))
    {
      moveX = -1f;
    }
    if (Input.GetKey(KeyCode.S))
    {
      moveY = -1f;
    }
    if (Input.GetKey(KeyCode.D))
    {
      moveX = +1f;
    }
    Vector3 moveDir = new Vector3(moveX, moveY).normalized;
    transform.position += moveDir * MoveSpeed * Time.deltaTime;
  }
  void FixedUpdate()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    { //menu
      MainMenu.paused = true;
      GameHandler.menu.GetComponent<MainMenu>().mainmenu.transform.GetChild(0).gameObject.SetActive(true);
      GameHandler.menu.GetComponent<MainMenu>().mainmenu.transform.GetChild(1).gameObject.SetActive(false);
      GameHandler.menu.GetComponent<MainMenu>().mainmenu.transform.GetChild(2).gameObject.SetActive(false);
      GameHandler.menu.SetActive(true);
      Time.timeScale = 0f;
    }
    inventory = GameHandler.inventory;
    if (inventory != null)
    {
      slots = inventory.transform.Find("Slots").transform;
    }

    //simples movimentação e rotação conforme posição do mouse
    if (!Fishing)
    {
      //movement.x = Input.GetAxisRaw("Horizontal");
      //movement.y = Input.GetAxisRaw("Vertical");
      HandleMovement();

      mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

      rb.MovePosition(rb.position + movement.normalized * MoveSpeed * Time.fixedDeltaTime);

      lookDir = mousePos - rb.position;
      float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
      rb.rotation = angle;
    }

    if (level >= 30) //trava no max level
    {
      baseexp = exp = level = 30;
    }

    if (exp >= baseexp && level < 30)
    {
      exp = Mathf.RoundToInt(exp - baseexp);
      level++;
      baseexp = Mathf.RoundToInt(level * 1.2f * 16);
      points++;
      if (level % 5 == 0)
      {
        points++;
      }
    }

    if (!canBow)
    { //slow ao atirar com o arco
      MoveSpeed = MoveSpeed * 0.75f;
    }

    if (weapon.CompareTag("Rod"))
    {
      weapon.transform.GetChild(1).gameObject.SetActive(true);
    }
    else
    {
      weapon.transform.GetChild(1).gameObject.SetActive(false);
    }

    //atributos (WIP)
    //Resistance = 1 - (10 / (10 + armor));
    if (weapon.CompareTag("IronSword") || weapon.CompareTag("SwordFish"))
    {
      baseSwordDamage = 7;
    }
    if (weapon.CompareTag("VileSword"))
    {
      baseSwordDamage = 11;
    }
    if (weapon.CompareTag("VileBow"))
    {
      baseBowDamage = 4.5f;
    }
    SwordDamage = Mathf.RoundToInt((baseSwordDamage * (1 + (strength * 0.13f))) + (strength * 1.2f));
    BowDamage = Mathf.RoundToInt((baseBowDamage * (1 + (dexterity * 0.06f))) + (strength * 0.1f) + (dexterity * 0.8f));
    attackspeed = 1 / (8.5f / (8.5f + agility));
    foreach (AnimationState state in anim)
    {
      state.speed = attackspeed;
    }
    health = Mathf.RoundToInt(basehealth + ((vigor * 4f) / 0.8f) + ((level * 2) / 0.8f));
    regen = baseregen * (10f / (10f + vigor));
    regenquant = Mathf.RoundToInt(0.02f + health * 0.012f);
    CritChance = 0.1f + ((0.1f * dexterity) / 2);
    CritMult = 1.5f + (dexterity * 0.05f);
    dashCD = basedashCD / (1 + (swiftness * 0.1f));
    dashrange = (1 + (swiftness * 0.07f));
    MoveSpeed = baseMoveSpeed * (1 + (swiftness * 0.01f));

    if (SelectedSlot != null)
    { //destaca o slot selecionado
      SelectedSlot.GetComponent<InventorySlot>().SlotImage.color = Color.yellow;
    }

    if (SelectedSlot == null || (SelectedSlot != null && SelectedSlot.transform.childCount == 0))
    { //remove o item da mão do player caso nenhum slot selecionado, ou nenhum item no slot
      weapon.GetComponent<SpriteRenderer>().sprite = null;
      weapon.tag = "Untagged";
    }
    else
    {
      weapon.GetComponent<SpriteRenderer>().sprite = SelectedSlot.transform.GetChild(0).gameObject.transform.GetComponent<Image>().sprite;
      weapon.tag = SelectedSlot.transform.GetChild(0).gameObject.tag;
    }
    //posição das mãos do personagem, conforme o item
    if (!Fishing)
    {
      if (!weapon.CompareTag("Untagged") && !weapon.CompareTag("GoldOre") && !weapon.CompareTag("IronOre") && !weapon.CompareTag("Rock") && !weapon.CompareTag("IronBar") && !weapon.CompareTag("GoldBar") && !weapon.CompareTag("Arrow") && !weapon.CompareTag("Fish") && !weapon.CompareTag("PufferFish") && !weapon.CompareTag("BlobFish") && !weapon.CompareTag("VileShard") && !weapon.CompareTag("Apple") && !weapon.CompareTag("RedRune") && !weapon.CompareTag("BlueRune") && !weapon.CompareTag("GreenRune") && !weapon.CompareTag("YellowRune"))
      {
        righth.transform.localPosition = new Vector3(-1.75f, -1.25f, 0);
      }
      else
      {
        righth.transform.localPosition = new Vector3(4, -2.5f, 0);
      }
      if (weapon.CompareTag("GoldOre") || weapon.CompareTag("IronOre") || weapon.CompareTag("Rock") || weapon.CompareTag("IronBar") || weapon.CompareTag("GoldBar") || weapon.CompareTag("Arrow") || weapon.CompareTag("Fish") || weapon.CompareTag("PufferFish") || weapon.CompareTag("BlobFish") || weapon.CompareTag("VileShard") || weapon.CompareTag("Apple") || weapon.CompareTag("RedRune") || weapon.CompareTag("BlueRune") || weapon.CompareTag("GreenRune") || weapon.CompareTag("YellowRune"))
      {
        righth.transform.localPosition = new Vector3(1, -1, 0);
      }
      if (weapon.CompareTag("Bow") || weapon.CompareTag("VileBow"))
      {
        lefth.transform.localPosition = new Vector3(-3.2f, -0.5f, 0);
        righth.transform.localPosition = new Vector3(1.75f, -3.75f, 0);
      }
      else
      {
        lefth.transform.localPosition = new Vector3(-4, -2.5f, 0);
      }
    }
    //botão de ação somente quando não estiver clicando em um menu ou arrastando um item
    if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && !GameHandler.Drag)
    {
      if (weapon.CompareTag(("Axe")) || weapon.CompareTag("VilePickAxe"))
      {
        if (canAxePick && !GameHandler.Audio.isPlaying)
        {
          GameHandler.Audio.PlayOneShot(Swing);
        }
        anim.Blend("AxePick", 1);
        Collider2D[] hitTarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, AxeTargetLayers);
        foreach (Collider2D target in hitTarget)
        {
          if (target.GetType() == typeof(BoxCollider2D) && target.gameObject.CompareTag("Tree") && target != null)
          {
            if (canAxePick == true) { StartCoroutine(AxePickDelay(target)); }
            return;
          }
          if (target.GetType() == typeof(PolygonCollider2D) && target.gameObject.CompareTag("Log") && target != null)
          {
            if (canAxePick == true) { StartCoroutine(AxePickDelay(target)); }
            return;
          }
        }
        Collider2D[] hitTarget2 = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayers);
        foreach (Collider2D target in hitTarget2)
        {
          if (target != null && target.gameObject.CompareTag("Enemy"))
          {
            if (canAttack == true) { StartCoroutine(AttackDelay(target, 2 + Mathf.RoundToInt(strength / 2))); }
            return;
          }
        }
      }
      if (weapon.CompareTag(("Pickaxe")) || weapon.CompareTag("VilePickAxe"))
      {
        if (canAxePick && !GameHandler.Audio.isPlaying)
        {
          GameHandler.Audio.PlayOneShot(Swing);
        }
        anim.Blend("AxePick", 1);
        Collider2D[] hitTarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, PickaxeTargetLayers);
        foreach (Collider2D target in hitTarget)
        {
          if (target.GetType() == typeof(BoxCollider2D) && target != null)
          {
            if (canAxePick == true) { StartCoroutine(AxePickDelay(target)); }
            return;
          }
        }
        if (weapon.CompareTag("VilePickAxe"))
        {
          Collider2D[] hitTarget2 = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, CrystalLayer);
          foreach (Collider2D target in hitTarget2)
          {
            if (target.GetType() == typeof(BoxCollider2D) && target != null)
            {
              if (canAxePick == true) { StartCoroutine(AxePickDelay(target)); }
              return;
            }
          }
        }
        Collider2D[] hitTarget3 = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayers);
        foreach (Collider2D target in hitTarget3)
        {
          if (target != null && target.gameObject.CompareTag("Enemy"))
          {
            if (canAttack == true) { StartCoroutine(AttackDelay(target, 2 + Mathf.RoundToInt(strength / 2))); }
            return;
          }
        }
      }
      if (weapon.CompareTag("WoodenSword") || weapon.CompareTag("IronSword") || weapon.CompareTag("VileSword") || weapon.CompareTag("SwordFish"))
      {
        if (canAttack && !GameHandler.Audio.isPlaying)
        {
          GameHandler.Audio.PlayOneShot(Swing);
        }
        anim.Blend("Sword", 1);
        Collider2D[] hitTarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayers);
        foreach (Collider2D target in hitTarget)
        {
          if (target != null && target.gameObject.CompareTag("Enemy"))
          {
            if (canAttack == true) { StartCoroutine(AttackDelay(target, SwordDamage)); }
            return;
          }
        }
      }
      if (weapon.CompareTag("Bow") || weapon.CompareTag("VileBow"))
      {
        if (GameHandler.CheckItems(1, "Arrow"))
        {
          anim.Blend("Bow", 1);
          if (canBow == true) { StartCoroutine(ShootBow()); }
        }
        else
        {
          StartCoroutine(GameHandler.Message("No arrows!"));
        }
      }
      if (weapon.CompareTag("PufferFish"))
      {
        if (canBomb && !GameHandler.Audio.isPlaying)
        {
          GameHandler.Audio.PlayOneShot(Swing);
        }
        if (canBomb == true) { StartCoroutine(ThrowBomb()); }
      }
      if (weapon.CompareTag(("Untagged")))
      { //tentei alternar entre as mãos para socar mas tive problemas com o attack delay
        // if(LastPunch == null){
        //     anim.clip = Punch1;
        //     LastPunch = Punch1;
        // }else{
        // if(LastPunch == Punch1){
        //     anim.clip = Punch2;
        //     LastPunch = Punch2;
        // }else{
        // if(LastPunch == Punch2){
        //     anim.clip = Punch1;
        //     LastPunch = Punch1;
        // }
        // }
        // }
        if (canAttack && !GameHandler.Audio.isPlaying)
        {
          GameHandler.Audio.PlayOneShot(Swing);
        }
        anim.Blend("Punch1", 1);
        Collider2D[] hitTarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayers);
        foreach (Collider2D target in hitTarget)
        {
          if (target != null && target.gameObject.CompareTag("Enemy"))
          {
            if (canAttack == true) { StartCoroutine(AttackDelay(target, 1 + Mathf.RoundToInt(strength / 2))); }
            return;
          }
        }
      }
    }
    if ((gameObject.transform.position.x + (0.33f * dashrange)) > 99.5f || (gameObject.transform.position.x - (0.33f * dashrange)) < 0.5f || (gameObject.transform.position.y + (0.33f * dashrange)) > 99.5f || (gameObject.transform.position.y - (0.33f * dashrange)) < 0.5f)
    {
      canDashWall = false;
    }
    else
    {
      canDashWall = true;
    }
  }

  void LateUpdate()
  { //dash conforme a direção pressionada ou local do mouse, instancia um rastro que some depois de 0.2sec
    if (Input.GetKeyDown(KeyCode.Space) && canDash && canDashWall)
    { //(WIP)>>adaptar para usuario poder mudar teclas
      GameHandler.Audio.PlayOneShot(DashSound);
      StartCoroutine(DashDelay());
      GameObject dash = null;
      if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
      {
        if (Input.GetKey(KeyCode.W))
        {
          if (dash == null)
          {
            dash = Instantiate(Dash, gameObject.transform.position, transform.rotation);
          }
          gameObject.transform.position += new Vector3(0, 0.33f * dashrange, 0);
          Destroy(dash, 0.2f);
        }
        if (Input.GetKey(KeyCode.A))
        {
          if (dash == null)
          {
            dash = Instantiate(Dash, gameObject.transform.position, transform.rotation);
          }
          gameObject.transform.position += new Vector3(-0.33f * dashrange, 0, 0);
          Destroy(dash, 0.2f);
        }
        if (Input.GetKey(KeyCode.S))
        {
          if (dash == null)
          {
            dash = Instantiate(Dash, gameObject.transform.position, transform.rotation);
          }
          gameObject.transform.position += new Vector3(0, -0.33f * dashrange, 0);
          Destroy(dash, 0.2f);
        }
        if (Input.GetKey(KeyCode.D))
        {
          if (dash == null)
          {
            dash = Instantiate(Dash, gameObject.transform.position, transform.rotation);
          }
          gameObject.transform.position += new Vector3(0.33f * dashrange, 0, 0);
          Destroy(dash, 0.2f);
        }
      }
      else
      { //direção do mouse caso nenhuma tecla pressionada
        if (dash == null)
        {
          dash = Instantiate(Dash, gameObject.transform.position, transform.rotation);
        }
        gameObject.transform.position += new Vector3(lookDir.normalized.x * 0.33f * dashrange, lookDir.normalized.y * 0.33f * dashrange, 0);
        Destroy(dash, 0.2f);
      }
    }
  }
  void Update()
  { //drop de itens
    if (!Fishing)
    {
      if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject() && canDrop)
      {
        canDrop = false;
        if (!weapon.CompareTag("Untagged"))
        {
          foreach (GameObject prefab in Prefabs)
          {
            if (weapon.CompareTag(prefab.tag))
            {
              Prefab = prefab;
              break;
            }
          }
          GameHandler.Audio.PlayOneShot(Drop);
          Instantiate(Prefab, attackPoint.position, Quaternion.identity);
          SelectedSlot.transform.GetComponent<InventorySlot>().ItemCount--;
          //ClearSelected();
        }
        canDrop = true;
        return;
      }
      if (Input.GetKeyDown(KeyCode.Alpha1))
      { //teclas numerais para selecionar itens do inventário
        Inventory(1);
      }
      if (Input.GetKeyDown(KeyCode.Alpha2))
      {
        Inventory(2);
      }
      if (Input.GetKeyDown(KeyCode.Alpha3))
      {
        Inventory(3);
      }
      if (Input.GetKeyDown(KeyCode.Alpha4))
      {
        Inventory(4);
      }
      if (Input.GetKeyDown(KeyCode.Alpha5))
      {
        Inventory(5);
      }
      if (Input.GetKeyDown(KeyCode.Alpha6))
      {
        Inventory(6);
      }
      if (Input.GetKeyDown(KeyCode.Alpha7))
      {
        Inventory(7);
      }
      if (Input.GetKeyDown(KeyCode.Alpha8))
      {
        Inventory(8);
      }
      if (Input.GetKeyDown(KeyCode.Alpha9))
      {
        Inventory(9);
      }
      if (Input.GetAxis("Mouse ScrollWheel") > 0 && canScroll && !EventSystem.current.IsPointerOverGameObject())
      {  //scroll para selecionar itens do inventário
        canScroll = false;
        if (quant < 9)
        {
          quant++;
        }
        else { quant = 1; }
        Inventory(quant);
        canScroll = true;
      }
      if (Input.GetAxis("Mouse ScrollWheel") < 0 && canScroll && !EventSystem.current.IsPointerOverGameObject())
      {
        canScroll = false;
        if (quant > 1)
        {
          quant--;
        }
        else { quant = 9; }
        Inventory(quant);
        canScroll = true;
      }
    }
    if (!weapon.CompareTag("Rod"))
    {
      Fishing = false;
    }
    if (!Fishing && weapon.CompareTag("Rod"))
    {
      StopAllCoroutines();
      if (!canDash)
      {
        StartCoroutine(DashDelay());
      }
    }
    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !GameHandler.Drag)
    {
      if (weapon.CompareTag("VileShard"))
      {
        GameHandler.Audio.PlayOneShot(Success);
        PlayerHealthBar.DmgText.GetPoint(gameObject.transform.position);
        points++;
        GameHandler.DestroyItem("VileShard", 1);
      }
      if (weapon.CompareTag("RedRune") || weapon.CompareTag("BlueRune") || weapon.CompareTag("GreenRune") || weapon.CompareTag("YellowRune"))
      {
        GameHandler.Audio.PlayOneShot(Swing);
        anim.Blend("AxePick", 1);
        Collider2D[] hitTarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, AltarLayer);
        foreach (Collider2D target in hitTarget)
        {
          if (target != null && target.gameObject.CompareTag("Altar"))
          {
            GameHandler.Altar.AddRune(weapon.tag);
            GameHandler.DestroyItem(weapon.tag, 1);
          }
        }
      }
      if (weapon.CompareTag("BlobFish"))
      {
        GameHandler.Audio.PlayOneShot(Success);
        Instantiate(BlueRune, attackPoint.transform.position, Quaternion.identity);
        GameHandler.DestroyItem("BlobFish", 1);
      }
      if (weapon.CompareTag("Fish") || weapon.CompareTag("Apple")) //cura com base na vida perdida
      {
        if (PlayerHealthBar.health < PlayerHealthBar.startingHealth)
        {
          if (PlayerHealthBar.health + Mathf.RoundToInt(((PlayerHealthBar.startingHealth - PlayerHealthBar.health) * 0.2f) + 10) < PlayerHealthBar.startingHealth)
          {
            if (weapon.CompareTag("Fish"))
            {
              GameHandler.DestroyItem("Fish", 1);
            }
            if (weapon.CompareTag("Apple"))
            {
              GameHandler.DestroyItem("Apple", 1);
            }
            PlayerHealthBar.DmgText.ShowHeal(gameObject.transform.position, Mathf.RoundToInt(((PlayerHealthBar.startingHealth - PlayerHealthBar.health) * 0.2f) + 10));
            PlayerHealthBar.health += Mathf.RoundToInt(((PlayerHealthBar.startingHealth - PlayerHealthBar.health) * 0.2f) + 10);
          }
          else
          {
            if (weapon.CompareTag("Fish"))
            {
              GameHandler.Audio.PlayOneShot(Success);
              GameHandler.DestroyItem("Fish", 1);
            }
            if (weapon.CompareTag("Apple"))
            {
              GameHandler.Audio.PlayOneShot(Success);
              GameHandler.DestroyItem("Apple", 1);
            }
            PlayerHealthBar.DmgText.ShowHeal(gameObject.transform.position, Mathf.RoundToInt((PlayerHealthBar.startingHealth - PlayerHealthBar.health)));
            PlayerHealthBar.health = PlayerHealthBar.startingHealth;
          }
        }
      }
      if (weapon.CompareTag("Rod"))
      {
        Collider2D[] hitLake = Physics2D.OverlapCircleAll(transform.TransformPoint(attackPoint.localPosition + new Vector3(0.02f, 0.25f, 0)), 0.02f, LakeLayers);//posicao do anzol
        if (hitLake.Length > 0)
        {
          if (!Fishing && !anim.isPlaying)
          {
            GameHandler.Audio.PlayOneShot(Swing);
            anim.Blend("FishThrow", 1);
            Fishing = true;
            StartCoroutine(Fish());
            return;
          }
          if (Fishing && !anim.isPlaying)
          {
            anim.Blend("FishGrab", 1);
            Fishing = false;
            if (hit)
            {
              hit = false;
              FishText.ShowText(gameObject, ":)", Color.green);
              GameHandler.Audio.PlayOneShot(Success);
              exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 8);
              int fish = Random.Range(0, FishPrefabs.Count - 1);
              Instantiate(FishPrefabs[fish], gameObject.transform.position - new Vector3(lookDir.normalized.x * 0.2f, lookDir.normalized.y * 0.2f, 0), Quaternion.identity);
              if (fish >= 9)
              { //index do blobfish
                FishPrefabs.RemoveRange(9, 2);
              }
            }
            else
            {
              GameHandler.Audio.PlayOneShot(Error);
              FishText.ShowText(gameObject, ":(", Color.red);
            }
            return;
          }
        }
      }
    }
  }

  IEnumerator Fish()
  {
    while (Fishing)
    {
      yield return new WaitForSeconds(Random.Range(3f, 10f));
      if (Fishing)
      {
        FishText.ShowText(gameObject, "!", Color.yellow);
        hit = true;
        yield return new WaitForSeconds(FishTime);
        hit = false;
        StartCoroutine(Fish());
        yield break;
      }
    }
  }

  void Inventory(int j)
  { //seleciona o slot 
    if (SelectedSlot != inventory.transform.Find("Slots").transform.Find("Slot (" + j + ")").gameObject)
    {
      ClearSelected();
      SelectedSlot = inventory.transform.Find("Slots").transform.Find("Slot (" + j + ")").gameObject;
    }
  }

  public static void ClearSelected()
  { //limpa a cor de seleção do slot anterior
    GameHandler GameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
    foreach (Transform child in GameHandler.Inventory1.transform.Find("SlotImages").transform)
    {
      if (child.GetComponent<Image>().color == Color.yellow)
      {
        child.GetComponent<Image>().color = new Color32(160, 115, 90, 255);
      }
    }
    foreach (Transform child in GameHandler.Inventory2.transform.Find("SlotImages").transform)
    {
      if (child.GetComponent<Image>().color == Color.yellow)
      {
        child.GetComponent<Image>().color = new Color32(190, 150, 130, 255);
      }
    }
  }

  void OnDrawGizmos()
  { //ver o range do player no gizmos
    if (attackPoint == null)
      return;
    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
  }
}