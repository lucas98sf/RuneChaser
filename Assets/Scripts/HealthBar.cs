using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HealthBar : MonoBehaviour
{
  public AudioClip Success;
  public AudioClip TakeHit;
  private Transform bar;
  private TextMeshProUGUI text;
  public float health = 5;
  private float startingHealth;
  private float percHealth = 1;
  public GameObject RockPrefab;
  public GameObject LogPrefab;
  public GameObject PlankPrefab;
  public GameObject GoldOrePrefab;
  public GameObject IronOrePrefab;
  public GameObject YellowRunePrefab;
  public GameObject VileShardPrefab;
  public float VileShardChance;
  public EnemyController EnemyController;
  private Transform location;
  public DmgText DmgText;
  [HideInInspector]
  public GameObject Player;
  public bool hit;
  public AudioClip Chop;
  public AudioClip Mine;

  private void Start()
  {
    Player = GameHandler.Player;
    if (gameObject.CompareTag("Enemy"))
    {
      health = EnemyController.health;
      location = gameObject.transform.parent.GetChild(1);
    }
    else
    {
      location = gameObject.transform.GetChild(1);
    }
    bar = location.transform.Find("Bar");
    startingHealth = health;
    text = location.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
    text.text = health.ToString() + "/" + startingHealth.ToString();
  }
  void OnMouseEnter()
  { //mostra as barras de vida apenas ao passar o mouse
    location.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255);
    location.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255);
    location.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 255);
    location.transform.GetChild(3).gameObject.SetActive(true);
  }

  void OnMouseExit()
  {
    location.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
    location.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
    location.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
    location.transform.GetChild(3).gameObject.SetActive(false);
  }

  void Update()
  {
    if (gameObject.CompareTag("Enemy") && !hit)
    {
      startingHealth = health = gameObject.transform.parent.GetChild(0).GetComponent<EnemyController>().health;
      bar.localScale = new Vector3(1, 1);
      text.text = health.ToString() + "/" + startingHealth.ToString();
    }
    if (health <= 0)
    {
      location.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
      location.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
      location.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 255);
      location.transform.GetChild(3).gameObject.SetActive(false);
      if (gameObject.CompareTag("Enemy"))
      {
        gameObject.GetComponent<Animation>().Blend("EnemyDead", 1, 0);
        health = 0.01f;
        DestroyImmediate(gameObject.GetComponentInChildren<CircleCollider2D>());
        Destroy(gameObject.transform.parent.gameObject, 0.5f);
        float chance = Random.value;
        if (chance <= VileShardChance)
        {
          Instantiate(VileShardPrefab, gameObject.transform.position, transform.rotation);
        }
        Player.GetComponent<PlayerController>().exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 16);
      }
      else
      {
        gameObject.GetComponent<Animation>().Blend("Dead", 1, 0);
        health = 0.01f;
        DestroyImmediate(gameObject.GetComponent<BoxCollider2D>());
        Destroy(gameObject, 0.33f);
      }
      //}
      if (gameObject.CompareTag("Tree"))
      { //para nao ficar instanciando enquanto reproduz a animação
        Instantiate(LogPrefab, gameObject.transform.position, transform.rotation);
        Player.GetComponent<PlayerController>().exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 4);
        return;
      }
      if (gameObject.CompareTag("Log"))
      {
        Instantiate(PlankPrefab, gameObject.transform.position + new Vector3(0.1f, 0.1f, 0), transform.rotation);
        Instantiate(PlankPrefab, gameObject.transform.position + new Vector3(-0.1f, -0.1f, 0), transform.rotation);
        return;
      }
      if (gameObject.CompareTag("GoldVein"))
      {
        for (int i = 0; i <= Random.Range(2, 7); i++)
        { //quantidade aleatória
          if (i % 2 > 0)
          { //dividir entre minério e pedras
            Instantiate(RockPrefab, gameObject.transform.position + new Vector3(Random.Range(-0.15f, 0.15f), i * 0.07f, 0), transform.rotation);
          }
          else
          {
            Instantiate(GoldOrePrefab, gameObject.transform.position + new Vector3(Random.Range(-0.15f, 0.15f), i * 0.07f, 0), transform.rotation); //locais diferentes
          }
        }
        Player.GetComponent<PlayerController>().exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 20);
        return;
      }
      if (gameObject.CompareTag("IronVein"))
      {
        for (int i = 0; i <= Random.Range(2, 7); i++)
        {
          if (i % 2 > 0)
          {
            Instantiate(RockPrefab, gameObject.transform.position + new Vector3(Random.Range(-0.15f, 0.15f), i * 0.07f, 0), transform.rotation);
          }
          else
          {
            Instantiate(IronOrePrefab, gameObject.transform.position + new Vector3(Random.Range(-0.15f, 0.15f), i * 0.07f, 0), transform.rotation);
          }
        }
        Player.GetComponent<PlayerController>().exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 10);
        return;
      }
      if (gameObject.CompareTag("Crystal"))
      {
        GameHandler.Audio.PlayOneShot(Success);
        Instantiate(YellowRunePrefab, gameObject.transform.position, transform.rotation);
        Player.GetComponent<PlayerController>().exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 50);
      }
    }
  }

  public void TakeDamage(float damage, bool crit)
  { //causa dano ao alvo
    if (gameObject.CompareTag("Enemy"))
    {
      hit = true;
      DmgText.ShowText(transform.position, Mathf.RoundToInt(damage).ToString(), false, crit);
      EnemyController.triggered = true;
      gameObject.GetComponent<Animation>().Blend("GetHit", 1, 0);
      GameHandler.Audio.PlayOneShot(TakeHit);
    }
    if (health > 0.01f)
    {
      health -= Mathf.RoundToInt(damage);
    }
    if (gameObject.CompareTag("Tree") || gameObject.CompareTag("Log"))
    {
      GameHandler.Audio.PlayOneShot(Chop);
    }
    if (gameObject.CompareTag("GoldVein") || gameObject.CompareTag("IronVein") || gameObject.CompareTag("Crystal"))
    {
      GameHandler.Audio.PlayOneShot(Mine);
    }
    percHealth = health / startingHealth;
    bar.localScale = new Vector3(percHealth, 1);
    text.text = health.ToString() + "/" + startingHealth.ToString();
  }
}