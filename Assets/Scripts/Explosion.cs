using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
  public AudioClip Boom;
  void Start()
  {
    GameHandler.Audio.PlayOneShot(Boom);
  }
  void Update()
  {

    Destroy(gameObject, 0.5f);
  }
}
