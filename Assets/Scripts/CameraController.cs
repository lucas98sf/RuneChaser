using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
  [HideInInspector]
  public Transform target;
  private GameHandler GameHandler;
  public float smoothTime = 0.3f;
  public float size = 0.255f;
  private Vector3 velocity = Vector3.zero;

  void FixedUpdate() //simples camera que segue o personagem e seu mouse, com suavização no movimento
  {
    Vector3 targetPosition = target.TransformPoint(new Vector3(0, size, -1)); //centralizada na frente do jogador

    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
  }
}