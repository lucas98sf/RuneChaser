using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Altar : MonoBehaviour
{
  public AudioClip Success;
  public GameObject GameWin;
  public GameObject UpArrow;
  public GameObject RedRune;
  public GameObject LeftArrow;
  public GameObject GreenRune;
  public GameObject DownArrow;
  public GameObject BlueRune;
  public GameObject RightArrow;
  public GameObject YellowRune;
  public GameObject FinalBoss;
  public GameObject Mark;
  public GameObject RedRunePrefab;
  public bool trigger = false;
  public bool trigger2 = false;

  void Update()
  {
    if (FinalBoss.transform.childCount == 0 && !trigger2)
    {
      trigger2 = true;
      Instantiate(RedRunePrefab, Mark.transform.position, Quaternion.identity);
      GameHandler.Audio.PlayOneShot(Success);
      GameHandler.Player.GetComponent<PlayerController>().exp += 999;
    }
    if (!LeftArrow.activeInHierarchy && !RightArrow.activeInHierarchy && !DownArrow.activeInHierarchy && !trigger)
    {
      trigger = true;
      FinalBoss.SetActive(true);
      UpArrow.SetActive(true);
    }
    if (RedRune.activeInHierarchy && GreenRune.activeInHierarchy && BlueRune.activeInHierarchy && YellowRune.activeInHierarchy)
    {
      GameWin.SetActive(true);
      GameWin.GetComponent<TextMeshProUGUI>().text = "You sucessfully brought balance to the world in " + GameHandler.timestring + "! Thanks for playing :)";
      Time.timeScale = 0;
    }
  }
  public void AddRune(string runetag)
  {
    if (runetag == "RedRune")
    {
      GameHandler.Audio.PlayOneShot(Success);
      RedRune.SetActive(true);
      UpArrow.SetActive(false);
    }
    if (runetag == "GreenRune")
    {
      GameHandler.Audio.PlayOneShot(Success);
      GreenRune.SetActive(true);
      LeftArrow.SetActive(false);
    }
    if (runetag == "BlueRune")
    {
      GameHandler.Audio.PlayOneShot(Success);
      BlueRune.SetActive(true);
      DownArrow.SetActive(false);
    }
    if (runetag == "YellowRune")
    {
      GameHandler.Audio.PlayOneShot(Success);
      YellowRune.SetActive(true);
      RightArrow.SetActive(false);
    }
  }

}
