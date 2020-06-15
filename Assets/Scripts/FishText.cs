using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FishText : MonoBehaviour
{
  public TextMeshPro text;
  public GameObject Player;
  void Start()
  {
    Player = GameHandler.Player;
  }

  void Update()
  {
    GetComponent<TextMeshPro>().color -= new Color(0, 0, 0, 0.02f);
    text.fontSize -= 0.02f;
    Destroy(gameObject, Player.GetComponent<PlayerController>().FishTime);
  }

  public void ShowText(GameObject player, string message, Color color)
  {
    Vector3 pos;
    if (message == "!")
    {
      pos = player.transform.position + new Vector3(-0.15f, 0.2f, 0);
    }
    else
    {
      pos = player.transform.position + new Vector3(0.15f, 0.2f, 0);
    }
    text.text = message;
    text.color = color;
    Instantiate(gameObject, pos, Quaternion.identity);
    text.fontSize = 2;
  }
}
