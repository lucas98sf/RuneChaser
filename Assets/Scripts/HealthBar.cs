using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HealthBar : MonoBehaviour
{
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
  public EnemyController EnemyController;
  private Transform location;
  public DmgText DmgText;
  public GameObject Player;

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
        DestroyImmediate(gameObject.GetComponentInChildren<BoxCollider2D>());
        Destroy(gameObject.transform.parent.gameObject, 0.5f);
        Player.GetComponent<PlayerController>().exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 16);
      }
      else
      {
        gameObject.GetComponent<Animation>().Blend("Dead", 1, 0);
        health = 0.01f;
        DestroyImmediate(gameObject.GetComponent<BoxCollider2D>());
        Destroy(gameObject, 0.33f);
      }
      if (gameObject.CompareTag("Tree"))
      { //para nao ficar instanciando enquanto reproduz a animação
        Instantiate(LogPrefab, gameObject.transform.position, transform.rotation);
        Player.GetComponent<PlayerController>().exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 2);
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
        for (int i = 0; i <= Random.Range(1, 7); i++)
        { //quantidade aleatória
          if (i % 2 > 0)
          { //dividir entre minério e pedras
            Instantiate(GoldOrePrefab, gameObject.transform.position + new Vector3(i / 10, (Random.Range(0, 5)) / 10, 0), transform.rotation); //locais diferentes
          }
          else
          {
            Instantiate(RockPrefab, gameObject.transform.position + new Vector3(i / 10, (Random.Range(0, 5)) / 10, 0), transform.rotation);
          }
        }
        Player.GetComponent<PlayerController>().exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 16);
        return;
      }
      if (gameObject.CompareTag("IronVein"))
      {
        for (int i = 0; i <= Random.Range(1, 7); i++)
        {
          if (i % 2 > 0)
          {
            Instantiate(IronOrePrefab, gameObject.transform.position + new Vector3(i / 10, (Random.Range(0, 5)) / 10, 0), transform.rotation);
          }
          else
          {
            Instantiate(RockPrefab, gameObject.transform.position + new Vector3(i / 10, (Random.Range(0, 5)) / 10, 0), transform.rotation);
          }
        }
        Player.GetComponent<PlayerController>().exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 8);
        return;
      }
    }
  }

  public void TakeDamage(float damage, bool crit)
  { //causa dano ao alvo
    if (gameObject.CompareTag("Enemy"))
    {
      DmgText.ShowText(transform.position, Mathf.RoundToInt(damage).ToString(), false, crit);
      EnemyController.triggered = true;
      gameObject.GetComponent<Animation>().Blend("GetHit", 1, 0);
    }
    if (health > 0.01f)
    {
      health -= Mathf.RoundToInt(damage);
    }
    percHealth = health / startingHealth;
    bar.localScale = new Vector3(percHealth, 1);
    text.text = health.ToString() + "/" + startingHealth.ToString();
  }
}