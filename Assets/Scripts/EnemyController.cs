using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [Header("Enemy Components")]
  public Rigidbody2D rb;
  private float startingMoveSpeed;
  private float step;
  public Transform attackPoint;
  GameObject righth;
  GameObject weapon;
  private Animation anim;
  [Header("Targets")]
  public LayerMask PlayerLayer;
  public GameObject Player;
  public GameObject PlayerHealthBar;
  private Vector2 playerPos;
  private float DistToPlayer = 1;
  [Header("Enemy Stats")]
  public float baseMoveSpeed;
  public float attackRange;
  public float arrowSpeed;
  public float basehealth;
  public float baseregen;
  public float strength;
  public float agility;
  public float vigor;
  public float dexterity;
  public float swiftness;
  [Header("In Game")]
  public float MoveSpeed;
  public int damage;
  public float attackspeed = 0;
  public float health;
  public float regen;
  public float regenquant = 1;
  public float armor = 0;
  public float Resistance;
  public float CritChance;
  public float CritMult;
  public bool triggered;
  private bool canAttack = true;

  void Start()
  {
    Player = GameHandler.Player;
    PlayerHealthBar = GameObject.Find("PlayerHealthBar");
    startingMoveSpeed = MoveSpeed;
    anim = GetComponent<Animation>();
    weapon = this.gameObject.transform.GetChild(2).gameObject;
    righth = this.gameObject.transform.GetChild(1).GetChild(1).gameObject;
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
    rb.velocity = Vector2.zero; //para evitar que o player gere uma força empurrando o enemy
    CritChance = 0.1f + (0.1f * dexterity / 2);

    transform.parent.GetChild(1).localPosition = transform.localPosition + new Vector3(0, -3.5f, 0);
    if (!weapon.CompareTag("Untagged"))
    {
      righth.transform.localPosition = new Vector3(-1.75f, -1.25f, 0);
    }
    if (DistToPlayer <= 0.3f)
    {
      MoveSpeed = 0f;
      AttackPlayer(); //ataca o player quando em distancia suficiente
    }
    else
    {
      MoveSpeed = startingMoveSpeed;
    }
    if (DistToPlayer <= 3f && triggered)
    {
      transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, step);
      playerPos = Player.transform.position;
      Vector2 lookDir = playerPos - rb.position;
      float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
      rb.rotation = angle;
    }
    else
    {
      triggered = false;
    }
    DistToPlayer = Vector3.Distance(Player.transform.position, transform.position);
    step = MoveSpeed * Time.deltaTime;
  }

  void AttackPlayer()
  {
    anim.Blend("Sword", 1f);
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

  IEnumerator AttackDelay(Collider2D target)
  {
    canAttack = false;
    yield return new WaitForSeconds(0.2f);
    if (target != null)
    {
      if (Random.value <= CritChance)
      {
        PlayerHealthBar.GetComponent<PlayerHealthBar>().TakeDamage(damage * CritMult, true);
      }
      else
      {
        PlayerHealthBar.GetComponent<PlayerHealthBar>().TakeDamage(damage, false);
      }
    }
    target.GetComponent<Rigidbody2D>().AddForce((target.transform.position - gameObject.transform.position).normalized * (damage * 5));
    yield return new WaitForSeconds(0.3f);
    canAttack = true;
  }
}