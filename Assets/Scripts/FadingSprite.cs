using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingSprite : MonoBehaviour
{
  void OnTriggerEnter2D(Collider2D other) //simples script para ser possivel ver o player através de objetos bloqueando a visão
  {
    if (!other.isTrigger)
    {
      Color tmp = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
      tmp.a = 0.6f;
      gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = tmp;
      gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 120;
    }
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (!other.isTrigger)
    {
      Color tmp = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
      tmp.a = 1;
      gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = tmp;
      gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 90;
    }
  }
}