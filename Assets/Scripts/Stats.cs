using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
  GameObject Points;
  GameObject Strength;
  GameObject Agility;
  GameObject Vigor;
  GameObject Dexterity;
  GameObject Swiftness;
  public float points = 1;
  public float strength;
  public float agility;
  public float vigor;
  public float dexterity;
  public float swiftness;
  public GameObject Player;
  private PlayerController player;
  public GameObject Infos;
  void Start()
  {
    Points = GameObject.Find("GameHandler/Canvas/Stats/Points/Points");
    Strength = GameObject.Find("GameHandler/Canvas/Stats/Points/Strength");
    Agility = GameObject.Find("GameHandler/Canvas/Stats/Points/Agility");
    Vigor = GameObject.Find("GameHandler/Canvas/Stats/Points/Vigor");
    Dexterity = GameObject.Find("GameHandler/Canvas/Stats/Points/Dexterity");
    Swiftness = GameObject.Find("GameHandler/Canvas/Stats/Points/Swiftness");
    Infos = GameObject.Find("GameHandler/Canvas/Stats/Infos");
    player = Player.GetComponent<PlayerController>();
  }

  void Update()
  {
    if (Player != null)
    {
      /*points = */
      Player.GetComponent<PlayerController>().points = points;
      /*strength = */
      Player.GetComponent<PlayerController>().strength = strength;
      /*agility = */
      Player.GetComponent<PlayerController>().agility = agility;
      /*vigor = */
      Player.GetComponent<PlayerController>().vigor = vigor;
      /*dexterity = */
      Player.GetComponent<PlayerController>().dexterity = dexterity;
      /*swiftness = */
      Player.GetComponent<PlayerController>().swiftness = swiftness;
      Points.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = points.ToString();
      Strength.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = strength.ToString();
      Agility.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = agility.ToString();
      Vigor.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = vigor.ToString();
      Dexterity.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dexterity.ToString();
      Swiftness.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = swiftness.ToString();
      if (points < 1)
      {
        Strength.transform.GetChild(1).GetComponent<Button>().interactable = false;
        Agility.transform.GetChild(1).GetComponent<Button>().interactable = false;
        Vigor.transform.GetChild(1).GetComponent<Button>().interactable = false;
        Dexterity.transform.GetChild(1).GetComponent<Button>().interactable = false;
        Swiftness.transform.GetChild(1).GetComponent<Button>().interactable = false;
      }
      else
      {
        Strength.transform.GetChild(1).GetComponent<Button>().interactable = true;
        Agility.transform.GetChild(1).GetComponent<Button>().interactable = true;
        Vigor.transform.GetChild(1).GetComponent<Button>().interactable = true;
        Dexterity.transform.GetChild(1).GetComponent<Button>().interactable = true;
        Swiftness.transform.GetChild(1).GetComponent<Button>().interactable = true;
      }
      Infos.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = player.armor.ToString() + " (" + player.Resistance.ToString("0.00") + "% reduction)";
      Infos.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = player.SwordDamage.ToString();
      Infos.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = player.BowDamage.ToString();
      Infos.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = (player.attackspeed * 1.25f).ToString() + " melee | " + player.attackspeed.ToString() + " ranged";
      Infos.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = player.health.ToString() + " (" + (player.health - player.basehealth - 2).ToString() + " extra)";
      Infos.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = player.regenquant.ToString() + " each " + player.regen.ToString("0.00") + " sec";
      Infos.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = (player.CritChance * 100).ToString() + "%";
      Infos.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = (player.CritMult * 100).ToString() + "%";
      Infos.transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>().text = player.dashCD.ToString("0.00") + " sec (" + ((1 - (player.dashCD / player.basedashCD)) * 100).ToString("0.00") + "% reduction)";
      Infos.transform.GetChild(9).GetChild(0).GetComponent<TextMeshProUGUI>().text = (player.dashrange * 0.33f * 10).ToString("0.00") + " unit(s)";
      Infos.transform.GetChild(10).GetChild(0).GetComponent<TextMeshProUGUI>().text = (player.MoveSpeed * 10).ToString("0.00") + " unit(s)/sec (" + ((((player.MoveSpeed / player.baseMoveSpeed)) * 100) - 100).ToString("0.00") + "% bonus)";
    }
  }

  public void IncreaseOnClick()
  {
    string statClicked = EventSystem.current.currentSelectedGameObject.transform.parent.name;
    if (points >= 1)
    {
      points--;
      if (statClicked == "Strength")
      {
        strength++;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
      }
      if (statClicked == "Agility")
      {
        agility++;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
      }
      if (statClicked == "Vigor")
      {
        vigor++;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
      }
      if (statClicked == "Dexterity")
      {
        dexterity++;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
      }
      if (statClicked == "Swiftness")
      {
        swiftness++;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
      }
    }
  }
}