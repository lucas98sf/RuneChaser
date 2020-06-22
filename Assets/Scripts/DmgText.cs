using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgText : MonoBehaviour
{
  public float duration;
  public float speed;
  public TextMeshPro text;
  [HideInInspector]
  public GameObject Player;

  void Start()
  {
    Player = GameHandler.Player;
    speed = 0.003f * (1 + Player.GetComponent<PlayerController>().attackspeed);
  }
  void Update() // texto de dano, com cores diferentes para player e enemy, e também em criticos
  {
    transform.position += new Vector3(0, speed, 0);
    if (Player != null)
    {
      speed = 0.003f * (1 + Player.GetComponent<PlayerController>().attackspeed);
    }
    GetComponent<TextMeshPro>().color -= new Color(0, 0, 0, 0.02f);
    Destroy(gameObject, duration); //duração determinada no inspector
  }

  public void GetPoint(Vector3 pos)
  {
    text.text = "+1 point!";
    text.color = Color.green;
    Instantiate(gameObject, pos, Quaternion.identity);
  }

  public void ShowHeal(Vector3 pos, int heal)
  {
    text.text = heal.ToString();
    text.color = Color.green;
    Instantiate(gameObject, pos, Quaternion.identity);
  }

  public void ShowText(Vector3 pos, string dmg, bool player, bool crit)
  {
    text.text = dmg;
    if (!player && !crit)
    {
      text.color = Color.white;
    }
    if (!player && crit)
    {
      text.fontSize = 2;
      text.color = Color.yellow;
    }
    if (player && !crit)
    {
      text.color = Color.red;
    }
    if (player && crit)
    {
      text.fontSize = 2;
      text.color = new Color(1, 0.5f, 0, 1);
    }
    Instantiate(gameObject, pos, Quaternion.identity);
    text.fontSize = 1;
  }
}
