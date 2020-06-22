using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Npc : MonoBehaviour
{
  public AudioClip Success;
  public Sprite happySprite;
  public Sprite leftH;
  public Sprite rigthH;
  public TextMeshPro PlankCounter;
  public int PlankNeeded;
  private int PlankQuant = 0;
  public TextMeshPro StickCounter;
  public int StickNeeded;
  private int StickQuant = 0;
  public TextMeshPro RockCounter;
  public int RockNeeded;
  private int RockQuant = 0;
  public GameObject Zone;
  public GameObject House;
  public GameObject Rune;
  public GameObject Baloon;

  void Update()
  {
    PlankCounter.text = PlankQuant + "/" + PlankNeeded;
    StickCounter.text = StickQuant + "/" + StickNeeded;
    RockCounter.text = RockQuant + "/" + RockNeeded;

    if (PlankQuant >= PlankNeeded && StickQuant >= StickNeeded && RockQuant >= RockNeeded)
    {
      GameHandler.Player.GetComponent<PlayerController>().exp += Mathf.RoundToInt(Random.Range(1f, 2f) * 50);
      PlankQuant = PlankNeeded = RockQuant = 0;
      Destroy(Zone);
      Destroy(Baloon);
      House.SetActive(true);
      GameHandler.Audio.PlayOneShot(Success);
      gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = happySprite;
      gameObject.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = leftH;
      gameObject.transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().sprite = rigthH;
      Instantiate(Rune, gameObject.transform.position + new Vector3(0, -0.2f, 0), Quaternion.identity);
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Plank") && PlankQuant < PlankNeeded)
    {
      PlankQuant++;
      Destroy(other.gameObject);
    }
    if (other.CompareTag("Stick") && StickQuant < StickNeeded)
    {
      StickQuant++;
      Destroy(other.gameObject);
    }
    if (other.CompareTag("Rock") && RockQuant < RockNeeded)
    {
      RockQuant++;
      Destroy(other.gameObject);
    }
  }
}