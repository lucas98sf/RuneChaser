using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
  public GameObject player;
  public GameObject mainmenu;
  public GameObject changecharacter;
  public GameObject display;

  void Awake()
  {
    DontDestroyOnLoad(gameObject);
  }
  public void GameStart()
  {
    gameObject.SetActive(false);
    GameHandler.menu = gameObject;
    SceneManager.LoadSceneAsync("Game");
  }
  public void GameContinue()
  {
    gameObject.SetActive(false);
    GameObject.Find("GameHandler").GetComponent<GameHandler>().cam.GetComponent<AudioListener>().enabled = true;
  }
  public void ChangeCharacter()
  {
    changecharacter.SetActive(true);
    mainmenu.SetActive(false);
  }
  public void BackMenu()
  {
    mainmenu.SetActive(true);
    changecharacter.SetActive(false);
  }
  public void ChangeColor() //muda a cor
  {
    player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors.normalColor;
    player.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors.normalColor;
    player.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = EventSystem.current.currentSelectedGameObject.GetComponent<Button>().colors.normalColor;
  }
  public void ChangeFace() //muda a face
  {
    player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
  }
  public void Exit()
  {
    Application.Quit();
  }

  void FixedUpdate()
  {
    if (SceneManager.GetSceneByName("Game").isLoaded)
    {
      mainmenu.transform.GetChild(0).gameObject.SetActive(true);
      mainmenu.transform.GetChild(1).gameObject.SetActive(false);
      mainmenu.transform.GetChild(2).gameObject.SetActive(false);
    }
    else
    {
      mainmenu.transform.GetChild(0).gameObject.SetActive(false);
      mainmenu.transform.GetChild(1).gameObject.SetActive(true);
      mainmenu.transform.GetChild(2).gameObject.SetActive(true);
    }

    GameObject.Find("Display/Body").GetComponent<Image>().sprite = player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
    GameObject.Find("Display/Body").GetComponent<Image>().color = player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;

    GameObject.Find("Display/Hands/Lefth").GetComponent<Image>().sprite = player.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
    GameObject.Find("Display/Hands/Lefth").GetComponent<Image>().color = player.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;

    GameObject.Find("Display/Hands/Righth").GetComponent<Image>().sprite = player.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
    GameObject.Find("Display/Hands/Righth").GetComponent<Image>().color = player.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color;
  }
}
