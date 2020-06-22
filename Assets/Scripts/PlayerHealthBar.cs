using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
  private Transform bar;
  public AudioClip TakeHit;
  private TextMeshProUGUI text;
  [HideInInspector]
  public GameObject Player;
  //[HideInInspector]
  public float startingHealth;
  //[HideInInspector]
  public float health;
  private float regen;
  private float regenquant;
  private float percHealth = 1;
  public DmgText DmgText;
  private float timer = 0;
  void Start()
  {
    regen = Player.GetComponent<PlayerController>().regen;
    bar = gameObject.transform.Find("Bar");
    startingHealth = Player.GetComponent<PlayerController>().basehealth;
    health = startingHealth + 2;
    text = transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
    text.text = health.ToString() + "/" + startingHealth.ToString();
  }
  void Update()
  {
    if (Player != null)
    {
      startingHealth = Player.GetComponent<PlayerController>().health;
      regen = Player.GetComponent<PlayerController>().regen;
      regenquant = Player.GetComponent<PlayerController>().regenquant;
    }

    percHealth = health / startingHealth;
    text.text = health.ToString() + "/" + startingHealth.ToString();
    if (startingHealth != 0)
    {
      bar.localScale = new Vector3(percHealth, 1);
    }

    if (percHealth < 1)
    {
      timer += Time.deltaTime;
      if (timer > regen)
      {
        if ((health + regenquant) <= startingHealth)
        {
          health += regenquant;
        }
        else
        {
          health = startingHealth;
        }
        timer = 0;
      }
    }

    if (percHealth < 0.5f)
    { //fica vermelha quando HP abaixo de 50%
      bar.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
    }
    else
    {
      bar.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.green;
    }
    if (health <= 0)
    {
      Time.timeScale = 0;
      Destroy(Player);
      GameObject.Find("GameHandler/Canvas/GameOver").SetActive(true);
    }
  }
  public void TakeDamage(float damage, bool crit)
  { //dano ao player
    Player.GetComponent<Animation>().Blend("GetHit", 1, 0);
    GameHandler.Audio.PlayOneShot(TakeHit);
    health -= Mathf.RoundToInt(damage);
    DmgText.ShowText(Player.transform.position, Mathf.RoundToInt(damage).ToString(), true, crit);
  }
}