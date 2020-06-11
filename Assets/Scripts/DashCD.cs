using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DashCD : MonoBehaviour
{
  private Transform bar;
  private TextMeshProUGUI text;
  private float dashCD;
  private bool canDash = true;
  private float percCD = 1f;
  private float elapsed = 0;
  public GameObject Player;
  bool CanDash;
  void Start()
  {
    dashCD = Player.GetComponent<PlayerController>().dashCD;
    bar = gameObject.transform.Find("Bar");
    text = transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
  }

  void Update()
  { //cooldown do dash do player, exibido na barra abaixo do hp
    if (Player != null)
    {
      dashCD = Player.GetComponent<PlayerController>().dashCD;
      canDash = Player.GetComponent<PlayerController>().canDash;
    }

    if (percCD >= 0.98)
    {
      bar.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
    }
    else
    {
      bar.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    if (!canDash)
    {
      elapsed += Time.deltaTime;
      text.text = elapsed.ToString("0.00") + "s/" + dashCD.ToString("0.00") + "s";
      percCD = elapsed / dashCD;
      bar.localScale = new Vector3(percCD, 1f);
    }
    else
    {
      elapsed = 0;
      percCD = 1f;
      text.text = dashCD.ToString("0.00") + "s/" + dashCD.ToString("0.00") + "s";
      bar.localScale = new Vector3(percCD, 1f);
    }

  }
}
