using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
  public GameObject Player;
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
    }
    Destroy(gameObject);
  }
  void Update()
  { //duração maxima de 1.5sec
    Destroy(gameObject, 1.5f);
  }
}