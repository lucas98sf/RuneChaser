using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
  [HideInInspector]
  public PlayerHealthBar PlayerHealthBar;
  [HideInInspector]
  public GameObject Player;
  [HideInInspector]
  public GameObject Enemy;
  void Start()
  {
    PlayerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<PlayerHealthBar>();
  }
  void OnCollisionEnter2D(Collision2D other)
  { //causa dano ao acertar, com chance de critico
    if (other.gameObject.CompareTag("Enemy"))
    {
      if (Random.value <= Player.GetComponent<PlayerController>().CritChance)
      {
        other.gameObject.GetComponentInChildren<HealthBar>().TakeDamage(Player.GetComponent<PlayerController>().BowDamage * Player.GetComponent<PlayerController>().CritMult, true);
      }
      else
      {
        other.gameObject.GetComponentInChildren<HealthBar>().TakeDamage(Player.GetComponent<PlayerController>().BowDamage, false);
      }
      Destroy(gameObject);
    }
    if (other.gameObject.CompareTag("Player"))
    {
      if (Enemy.GetComponent<EnemyController>() != null)
      {
        if (Random.value <= Enemy.GetComponent<EnemyController>().CritChance)
        {
          PlayerHealthBar.TakeDamage(Enemy.GetComponent<EnemyController>().BowDamage * Enemy.GetComponent<EnemyController>().CritMult, true);
        }
        else
        {
          PlayerHealthBar.TakeDamage(Enemy.GetComponent<EnemyController>().BowDamage, false);
        }
      }
    }
    Destroy(gameObject);
  }
  void Update()
  { //duração maxima de 1.5sec
    Destroy(gameObject, 1.5f);
  }
}