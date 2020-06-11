using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
  public GameObject player;

  public void BackMenu()
  {
    SceneManager.LoadScene("Main Menu");
  }
  public void GameStart()
  {
    SceneManager.LoadScene("Game");
  }
  public void ChooseCharacter()
  {
    SceneManager.LoadScene("Choose Character");
  }
  public void Exit()
  {
    Application.Quit();
  }

  public void ChangeCharacter() //muda a cor
  {
    player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors.normalColor;
    player.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors.normalColor;
    player.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors.normalColor;

  }
  public void ChangeFace() //muda a face
  {
    player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
  }
  void FixedUpdate()
  {
    GameObject.Find("Display/Body").GetComponent<Image>().sprite = player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
    GameObject.Find("Display/Body").GetComponent<Image>().color = player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;

    GameObject.Find("Display/Hands/Lefth").GetComponent<Image>().sprite = player.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
    GameObject.Find("Display/Hands/Lefth").GetComponent<Image>().color = player.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;

    GameObject.Find("Display/Hands/Righth").GetComponent<Image>().sprite = player.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
    GameObject.Find("Display/Hands/Righth").GetComponent<Image>().color = player.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color;
  }
}
