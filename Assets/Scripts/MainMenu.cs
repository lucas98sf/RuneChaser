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
  public GameObject quitconfirmation;
  public GameObject help;
  public GameObject options;
  public static bool paused = false;
  public GameObject Audio;
  public AudioSource Music;
  public Slider MusicVolume;
  public Slider FXVolume;

  void Awake()
  {
    DontDestroyOnLoad(gameObject);
    DontDestroyOnLoad(Audio);
  }
  public void GameStart()
  {
    gameObject.SetActive(false);
    GameHandler.menu = gameObject;
    SceneManager.LoadSceneAsync("Game");
  }
  public void GameContinue()
  {
    Time.timeScale = 1f;
    gameObject.SetActive(false);
    mainmenu.transform.GetChild(0).gameObject.SetActive(false);
    mainmenu.transform.GetChild(1).gameObject.SetActive(true);
    mainmenu.transform.GetChild(2).gameObject.SetActive(true);
  }
  public void ChangeCharacter()
  {
    changecharacter.SetActive(true);
    mainmenu.SetActive(false);
  }
  public void Help()
  {
    help.SetActive(true);
    mainmenu.SetActive(false);
  }
  public void Options()
  {
    options.SetActive(true);
    mainmenu.SetActive(false);
  }
  public void BackMenu()
  {
    mainmenu.SetActive(true);
    EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
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
    quitconfirmation.SetActive(true);
  }
  public void YesQuit()
  {
    Application.Quit();
  }
  public void NoQuit()
  {
    quitconfirmation.SetActive(false);
  }
  void FixedUpdate()
  {
    Music.volume = MusicVolume.value;
    GameObject.Find("Display/Body").GetComponent<Image>().sprite = player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
    GameObject.Find("Display/Body").GetComponent<Image>().color = player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;

    GameObject.Find("Display/Hands/Lefth").GetComponent<Image>().sprite = player.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite;
    GameObject.Find("Display/Hands/Lefth").GetComponent<Image>().color = player.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;

    GameObject.Find("Display/Hands/Righth").GetComponent<Image>().sprite = player.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite;
    GameObject.Find("Display/Hands/Righth").GetComponent<Image>().color = player.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color;
  }
}
