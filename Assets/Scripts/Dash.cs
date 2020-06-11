using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
  public GameObject Player;
  void Start() //rastro que o player deixa na posição anterior ao dash, tem a cor correspondente à cor do player
  {
    GetComponent<SpriteRenderer>().color = Player.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
  }
}
