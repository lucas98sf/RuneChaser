using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [Header("Enemy Components")]
  public bool boss = false;
  public AudioClip Swing;
  public AudioClip Bow;
  public Rigidbody2D rb;
  private float step;
  public Transform attackPoint;
  GameObject righth;
  GameObject lefth;
  GameObject weapon;
  private Animation anim;
  public Sprite WoodenSwordSprite;
  public Sprite IronSwordSprite;
  public Sprite VileSwordSprite;
  public Sprite WoodenBowSprite;
  public Sprite VileBowSprite;
  public GameObject Arrow;

  [Header("Targets")]
  public LayerMask PlayerLayer;
  [HideInInspector]
  public GameObject Player;
  private GameObject PlayerHealthBar;
  private Vector2 playerPos;
  private float DistToPlayer = 99;
  [Header("Enemy Stats")]
  public float baseSwordDamage;
  public float baseBowDamage;
  public float baseMoveSpeed;
  public float attackRange;
  public float arrowSpeed;
  public float basehealth;
  public float baseregen;
  public int strength;
  public int agility;
  public int vigor;
  public int dexterity;
  public int swiftness;
  [Header("In Game")]
  public float SwordDamage;
  public float BowDamage;
  public float MoveSpeed;
  public float attackspeed = 0;
  public float health;
  private float regen;
  public float regenquant = 1;
  //public float armor = 0;
  //public float Resistance;
  public float CritChance;
  public float CritMult;
  public bool triggered;
  private bool canAttack = true;
  private bool canBow = true;

  void Start()
  {
    Player = GameHandler.Player;
    PlayerHealthBar = GameObject.Find("PlayerHealthBar");
    health = basehealth;
    MoveSpeed = baseMoveSpeed;
    anim = GetComponent<Animation>();
    weapon = this.gameObject.transform.GetChild(2).gameObject;
    righth = this.gameObject.transform.GetChild(1).GetChild(1).gameObject;
    lefth = this.gameObject.transform.GetChild(1).GetChild(0).gameObject;
    Weapon();
  }
  void Weapon()
  {
    if (!boss)
    {
      float chance = Random.value;
      if (chance <= 0.7f)
      {
        weapon.GetComponent<SpriteRenderer>().sprite = WoodenSwordSprite;
        weapon.tag = "WoodenSword";
      }
      else
      {
        weapon.GetComponent<SpriteRenderer>().sprite = WoodenBowSprite;
        weapon.tag = "Bow";
      }
    }
  }
  void OnTriggerEnter2D(Collider2D other)
  {  //segue o player quando ele entra em seu range
    if (other.gameObject == Player)
    {
      triggered = true;
    }
  }
  void FixedUpdate()
  {
    if (!boss)
    {
      strength = Mathf.RoundToInt(GameHandler.timer / 300); //a cada 5min
      agility = Mathf.RoundToInt(GameHandler.timer / 300);
      vigor = Mathf.RoundToInt(GameHandler.timer / 300);
      dexterity = Mathf.RoundToInt(GameHandler.timer / 300);
      swiftness = Mathf.RoundToInt(GameHandler.timer / 300);
    }
    else
    {
      strength = Mathf.RoundToInt(GameHandler.timer / 180); //a cada 3min
      agility = Mathf.RoundToInt(GameHandler.timer / 180);
      vigor = 15 + Mathf.RoundToInt(GameHandler.timer / 180);
      dexterity = Mathf.RoundToInt(GameHandler.timer / 180);
      swiftness = 15 + Mathf.RoundToInt(GameHandler.timer / 180);
    }

    if ((GameHandler.timer / 60) >= 7) // 7min (10 no total)
    {
      gameObject.transform.localScale = new Vector3(20, 20, 1);
      if (weapon.CompareTag("WoodenSword"))
      {
        weapon.tag = "IronSword";
        weapon.GetComponent<SpriteRenderer>().sprite = IronSwordSprite;
      }
      else
      {
        baseBowDamage = 2.5f;
      }
    }
    // if ((timer / 60) >= 11) //11min
    // {
    //   gameObject.transform.localScale = new Vector3(22,22,1);
    //   if (weapon.CompareTag("IronSword"))
    //   {
    //     weapon.tag = "VileSword";
    //     weapon.GetComponent<SpriteRenderer>().sprite = VileSwordSprite;
    //   }
    //   if (weapon.CompareTag("Bow"))
    //   {
    //     weapon.tag = "VileBow";
    //     weapon.GetComponent<SpriteRenderer>().sprite = VileBowSprite;
    //   }
    // }

    DistToPlayer = Vector3.Distance(Player.transform.position, transform.position);
    step = MoveSpeed * Time.deltaTime;
    rb.velocity = Vector2.zero; //para evitar que o player gere uma força empurrando o enemy

    //Resistance = 1 - (10 / (10 + armor));
    if (weapon.CompareTag("IronSword"))
    {
      baseSwordDamage = 4;
    }
    if (weapon.CompareTag("VileSword"))
    {
      baseSwordDamage = 7;
    }
    if (weapon.CompareTag("VileBow"))
    {
      baseBowDamage = 4.5f;
    }
    SwordDamage = Mathf.RoundToInt((baseSwordDamage * (1 + (strength * 0.13f))) + (strength * 1.2f));
    BowDamage = Mathf.RoundToInt((baseBowDamage * (1 + (dexterity * 0.06f))) + (strength * 0.1f) + (dexterity * 0.8f));
    if (weapon.CompareTag("Bow") || weapon.CompareTag("VileBow"))
    {
      attackspeed = 0.5f / (8.5f / (8.5f + agility));
      foreach (AnimationState state in anim)
      {
        state.speed = attackspeed;
      }
    }
    else
    {
      attackspeed = 1 / (8.5f / (8.5f + agility));
      foreach (AnimationState state in anim)
      {
        state.speed = attackspeed;
      }
    }
    health = Mathf.RoundToInt(basehealth + ((vigor * 4f) / 0.8f) + (GameHandler.timer / 60));
    regen = baseregen * (10f / (10f + vigor));
    regenquant = Mathf.RoundToInt(0.02f + health * 0.012f);
    CritChance = 0.1f + ((0.1f * dexterity) / 2);
    CritMult = 1.5f + (dexterity * 0.05f);

    transform.parent.GetChild(1).localPosition = transform.localPosition + new Vector3(0, -3.5f, 0);
    if (!weapon.CompareTag("Bow") && !weapon.CompareTag("VileBow"))
    {
      righth.transform.localPosition = new Vector3(-1.75f, -1.25f, 0);
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
    if (DistToPlayer <= 0.35f)
    {
      MoveSpeed = 0;
      AttackPlayer(); //ataca o player quando em distancia suficiente
    }
    else
    {
      if (DistToPlayer <= 1.4f && (weapon.CompareTag("Bow") || weapon.CompareTag("VileBow")))
      {
        if (canBow)
        {
          MoveSpeed = baseMoveSpeed * (1 + (swiftness * 0.01f));
        }
        else
        {
          MoveSpeed = MoveSpeed * 0.75f;
        }
        AttackPlayer();
      }
      else
      {
        MoveSpeed = baseMoveSpeed * (1 + (swiftness * 0.01f));
      }
    }
    if (DistToPlayer <= 2.2f && triggered)
    {
      transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, step);
      playerPos = Player.transform.position;
      Vector2 lookDir = playerPos - rb.position;
      float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
      rb.rotation = angle;
    }
    else
    {
      triggered = false;
    }
  }

  void AttackPlayer()
  {
    if (weapon.CompareTag("WoodenSword") || weapon.CompareTag("IronSword") || weapon.CompareTag("VileSword"))
    {
      anim.Blend("Sword", 1);
      Collider2D[] hitTarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, PlayerLayer);
      foreach (Collider2D target in hitTarget)
      {
        if (target != null && target.gameObject.CompareTag("Player"))
        {
          if (canAttack == true) { StartCoroutine(AttackDelay(target)); }
          return;
        }
      }
    }
    else
    {
      anim.Blend("Bow", 1);
      if (canBow == true) { StartCoroutine(ShootBow()); }
    }
  }

  IEnumerator AttackDelay(Collider2D target)
  {
    canAttack = false;
    GameHandler.Audio.PlayOneShot(Swing);
    yield return new WaitForSeconds(0.2f);
    if (target != null)
    {
      if (Random.value <= CritChance)
      {
        PlayerHealthBar.GetComponent<PlayerHealthBar>().TakeDamage(SwordDamage * CritMult, true);
      }
      else
      {
        PlayerHealthBar.GetComponent<PlayerHealthBar>().TakeDamage(SwordDamage, false);
      }
    }
    yield return new WaitForSeconds(0.3f);
    canAttack = true;
  }

  IEnumerator ShootBow()
  {
    canBow = false;
    yield return new WaitForSeconds(0.77f * (1 / attackspeed));
    GameHandler.Audio.PlayOneShot(Bow);
    GameObject arrow = Instantiate(Arrow, attackPoint.position, attackPoint.rotation);
    arrow.GetComponent<Arrow>().Enemy = gameObject;
    Rigidbody2D arrowrb = arrow.GetComponent<Rigidbody2D>();
    arrowrb.AddForce(attackPoint.up * arrowSpeed, ForceMode2D.Impulse);
    yield return new WaitForSeconds(0.1f * (1 / attackspeed));
    canBow = true;
  }

}