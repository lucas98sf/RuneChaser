using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
  [HideInInspector]
  public GameObject Player;
  public GameObject Explosion;
  bool Exp = false;
  float timer = 0;

  void OnTriggerEnter2D(Collider2D other)
  { //causa dano ao acertar, com chance de critico
    if (other.gameObject.CompareTag("Enemy") && !other.isTrigger)
    {
      if (Random.value <= Player.GetComponent<PlayerController>().CritChance)
      {
        other.gameObject.GetComponentInChildren<HealthBar>().TakeDamage((Player.GetComponent<PlayerController>().BowDamage * 3 + 15) * Player.GetComponent<PlayerController>().CritMult, true);
      }
      else
      {
        other.gameObject.GetComponentInChildren<HealthBar>().TakeDamage(Player.GetComponent<PlayerController>().BowDamage * 3 + 15, false);
      }
      if (!Exp)
      {
        Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
        Exp = true;
      }
      Destroy(gameObject, 0.1f);
    }
  }
  void Update()
  {
    timer += Time.deltaTime;
    transform.Rotate(new Vector3(0, 0, Time.deltaTime * 3600), Space.Self);
    //duração maxima de 1.5sec
    if (timer >= 1.5f)
    {
      Instantiate(Explosion, gameObject.transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
  }
}
