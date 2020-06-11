using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
  public Transform target;
  public GameHandler GameHandler;
  public float smoothTime = 0.3F;
  public float size = 0.255f;
  private Vector3 velocity = Vector3.zero;

  void FixedUpdate() //simples camera que segue o personagem e seu mouse, com suavização no movimento
  {
    Vector3 targetPosition = target.TransformPoint(new Vector3(0f, size, -1f)); //centralizada na frente do jogador

    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
  }
}