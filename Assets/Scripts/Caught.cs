using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Caught : MonoBehaviour
{
    public float duration;
    public TextMeshPro text;
    public GameObject Player;
    void Start()
    {
        Player = GameHandler.Player;
    }

    void Update()
    {
        Destroy(gameObject, Player.GetComponent<PlayerController>().FishTime);
    }
}
