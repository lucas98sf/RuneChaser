using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
  public Camera cam;

  [Header("Player Components")]
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
  GameObject SelectedSlot;
  Animation anim;
  public float FishTime;
  [Header("Prefabs")]
  public GameObject[] Prefabs;
  public GameObject Caught;
  private GameObject Prefab;
  public GameObject Arrow;
  [Header("Targets")]
  public LayerMask AxeTargetLayers;
  public LayerMask PickaxeTargetLayers;
  public LayerMask EnemyLayers;
  [Header("Player Stats")]
  public float baseMoveSpeed;
  public float attackRange;
  public float arrowSpeed;
  public int baseBowDamage;
  public int baseSwordDamage;
  private float AxeDamage = 1;
  private float PickaxeDamage = 1;
  public float basedashCD;
  public float basehealth;
  public float baseregen;
  public float strength; //atributos, work in progress
  public float agility;
  public float vigor;
  public float dexterity;
  public float swiftness;
  [Header("In Game")]
  public float MoveSpeed;
  public int BowDamage;
  public int SwordDamage;
  public float attackspeed = 0;
  public float dashCD;
  public float dashrange = 0;
  public float health;
  public float regen;
  public float regenquant = 1;
  public int level = 1;
  public float points = 1;
  public float baseexp;
  public float exp;
  public float armor = 0;
  public float Resistance;
  public float CritChance;
  public float CritMult;
  private bool canAxePick = true;
  private bool canDrop = true;
  private bool canScroll = true;
  private bool canAttack = true;
  private bool canBow = true;
  private bool Fishing;
  private bool hit;
  [HideInInspector] public bool canDash = true;
  [HideInInspector] public bool canDashWall = true;
  private int quant = 0;

  void Start()
  {
    cam = Camera.main;
    anim = GetComponent<Animation>();
    weapon = this.gameObject.transform.GetChild(2).gameObject;
    righth = this.gameObject.transform.GetChild(1).GetChild(1).gameObject;
    lefth = this.gameObject.transform.GetChild(1).GetChild(0).gameObject;
    slots = GameObject.Find("GameHandler/Canvas/Inventory/Slots").transform;
    baseexp = Mathf.RoundToInt(level * 1.2f * 32);
  }

  IEnumerator AttackDelay(Collider2D target, float damage)
  { //delay no ataque
    canAttack = false;
    yield return new WaitForSeconds(0.25f * (8.5f / (8.5f + agility)));
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
    }
    yield return new WaitForSeconds(0.4f * (8.5f / (8.5f + agility)));
    canAttack = true;
  }

  IEnumerator AxePickDelay(Collider2D target)
  {
    canAxePick = false;
    yield return new WaitForSeconds(0.25f * (8.5f / (8.5f + agility)));
    if (target != null)
    {
      target.gameObject.GetComponent<Animation>().Play();
    }
    yield return new WaitForSeconds(0.4f * (8.5f / (8.5f + agility)));
    if (target != null)
    {
      if (weapon.CompareTag("Axe"))
      {
        target.GetComponent<HealthBar>().TakeDamage(AxeDamage, false);
      }
      if (weapon.CompareTag("Pickaxe"))
      {
        target.GetComponent<HealthBar>().TakeDamage(PickaxeDamage, false);
      }
    }
    canAxePick = true;
  }

  IEnumerator ShootBow()
  {
    canBow = false;
    yield return new WaitForSeconds(0.77f * (8.5f / (8.5f + agility)));
    GameObject arrow = Instantiate(Arrow, attackPoint.position, attackPoint.rotation);
    Rigidbody2D arrowrb = arrow.GetComponent<Rigidbody2D>();
    arrowrb.AddForce(attackPoint.up * arrowSpeed, ForceMode2D.Impulse);
    yield return new WaitForSeconds(0.1f * (8.5f / (8.5f + agility)));
    canBow = true;
  }

  IEnumerator Fish()
  {
    Fishing = true;
    yield return new WaitForSeconds(Random.Range(3f, 10f));
    Instantiate(Caught, gameObject.transform.position + new Vector3(0, 0.42f, 0), Quaternion.identity);
    if (Input.GetMouseButton(0))
    {
      hit = true;
      Fishing = false;
    }
    yield return new WaitForSeconds(FishTime);
    hit = false;
    anim["FishThrow"].enabled = false;
    Fishing = false;
  }

  IEnumerator DashDelay()
  {
    canDash = false;
    yield return new WaitForSeconds(dashCD);
    canDash = true;
  }

  void FixedUpdate()
  {
    //simples movimentação e rotação conforme posição do mouse
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");

    mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

    rb.MovePosition(rb.position + movement.normalized * MoveSpeed * Time.fixedDeltaTime);

    lookDir = mousePos - rb.position;
    float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    rb.rotation = angle;

    if (exp >= baseexp)
    {
      exp = Mathf.RoundToInt(exp - baseexp);
      level++;
      baseexp = Mathf.RoundToInt(level * 1.2f * 32);
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
    Resistance = 1 - (10 / (10 + armor));
    SwordDamage = Mathf.RoundToInt((baseSwordDamage * (1 + strength * 0.13f)) + (strength * 1.2f));
    if (weapon.CompareTag("IronSword"))
    {
      baseSwordDamage = 7;
    }
    if (weapon.CompareTag("GoldenSword"))
    {
      baseSwordDamage = 11;
    }
    BowDamage = Mathf.RoundToInt((baseBowDamage * (1 + (dexterity * 0.06f))) + (strength * 0.1f) + (dexterity * 0.8f));
    attackspeed = 1 / (8.5f / (8.5f + agility));
    foreach (AnimationState state in anim)
    {
      state.speed = attackspeed;
    }
    health = Mathf.RoundToInt(basehealth + ((vigor * 4) / 0.8f) + ((level * 2) / 0.8f));
    regen = baseregen * (10 / (10 + vigor));
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
      if (!weapon.CompareTag("Untagged") && !weapon.CompareTag("GoldOre") && !weapon.CompareTag("IronOre") && !weapon.CompareTag("Rock") && !weapon.CompareTag("IronBar") && !weapon.CompareTag("GoldBar"))
      {
        righth.transform.localPosition = new Vector3(-1.75f, -1.25f, 0);
      }
      else
      {
        righth.transform.localPosition = new Vector3(4f, -2.5f, 0);
      }
      if (weapon.CompareTag("GoldOre") || weapon.CompareTag("IronOre") || weapon.CompareTag("Rock") || weapon.CompareTag("IronBar") || weapon.CompareTag("GoldBar"))
      {
        righth.transform.localPosition = new Vector3(1f, -1f, 0);
      }
      if (weapon.CompareTag("Bow"))
      {
        lefth.transform.localPosition = new Vector3(-3.2f, -0.5f, 0);
        righth.transform.localPosition = new Vector3(1.75f, -3.75f, 0);
      }
      else
      {
        lefth.transform.localPosition = new Vector3(-4f, -2.5f, 0);
      }
    }
    //botão de ação somente quando não estiver clicando em um menu ou arrastando um item
    if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && !GameHandler.Drag)
    {
      if (weapon.CompareTag(("Axe")))
      {
        anim.Blend("AxePick", 1f);
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
            if (canAttack == true) { StartCoroutine(AttackDelay(target, 1 + AxeDamage + Mathf.RoundToInt(strength / 2))); }
            return;
          }
        }
      }
      if (weapon.CompareTag(("Pickaxe")))
      {
        anim.Blend("AxePick", 1f);
        Collider2D[] hitTarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, PickaxeTargetLayers);
        foreach (Collider2D target in hitTarget)
        {
          if (target.GetType() == typeof(BoxCollider2D) && target != null)
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
            if (canAttack == true) { StartCoroutine(AttackDelay(target, 1 + PickaxeDamage + Mathf.RoundToInt(strength / 2))); }
            return;
          }
        }
      }
      if (weapon.CompareTag("WoodenSword") || weapon.CompareTag("IronSword") || weapon.CompareTag("GoldenSword"))
      {
        anim.Blend("Sword", 1f);
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
      if (weapon.CompareTag("Bow"))
      {
        anim.Blend("Bow", 1f);
        if (canBow == true) { StartCoroutine(ShootBow()); }
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
        anim.Blend("Punch1", 1f);
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
    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !GameHandler.Drag)
    {
      if (weapon.CompareTag("Rod"))
      {
        if (Fishing)
        {
          Fishing = false;
          anim.Blend("FishGrab", 1f);
          if (hit)
          {
            Instantiate(Prefabs[5], gameObject.transform.position + new Vector3(0.2f, 0, 0), Quaternion.identity);
          }
        }
        if (!Fishing)
        {
          anim.Blend("FishThrow", 1f);
          StartCoroutine(Fish());
        }
        return;
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
  { //drop de itens (atualmente no botão direito, mas se houver alguma ferramenta com uso secundario mudarei para o Q), pressionar shift faz com que drope enquanto o botão esteja pressionado
    if ((Input.GetMouseButtonDown(1)) && !EventSystem.current.IsPointerOverGameObject() && canDrop ||
    (Input.GetMouseButton(1)) && (Input.GetKey(KeyCode.LeftShift)) && !EventSystem.current.IsPointerOverGameObject() && canDrop)
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
    if (Input.GetAxis("Mouse ScrollWheel") > 0f && canScroll && !EventSystem.current.IsPointerOverGameObject())
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
    if (Input.GetAxis("Mouse ScrollWheel") < 0f && canScroll && !EventSystem.current.IsPointerOverGameObject())
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

  void Inventory(int j)
  { //seleciona o slot
    if (SelectedSlot != GameObject.Find("GameHandler/Canvas/Inventory/Slots/Slot (" + j + ")"))
    {
      ClearSelected();
      SelectedSlot = GameObject.Find("GameHandler/Canvas/Inventory/Slots/Slot (" + j + ")");
    }
  }

  public static void ClearSelected()
  { //limpa a cor de seleção do slot anterior
    foreach (Transform child in GameObject.Find("GameHandler/Canvas/Inventory/SlotImages").transform)
    {
      if (child.GetComponent<Image>().color == Color.yellow)
      {
        child.GetComponent<Image>().color = new Color32(160, 115, 90, 255);
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