using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class XPBar : MonoBehaviour
{
  private Transform bar;
  private TextMeshProUGUI text;
  public GameObject Player;
  public float baseexp;
  public float exp;
  public int level;
  private float percXP;
  void Start()
  {
    bar = transform.GetChild(0).GetChild(2);
    text = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
  }

  void Update()
  {
    if (Player != null)
    {
      baseexp = Player.GetComponent<PlayerController>().baseexp;
      exp = Player.GetComponent<PlayerController>().exp;
      level = Player.GetComponent<PlayerController>().level;
    }
    text.text = level.ToString();
    percXP = exp / baseexp;
    bar.localScale = new Vector3(percXP, 1f);
  }
}
