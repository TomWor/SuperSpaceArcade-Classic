using UnityEngine;
using System.Collections;

public class HealthGUI : MonoBehaviour {

  private Player player;


  void Start () {
    player = GameObject.FindWithTag("Player").GetComponent<Player>();
  }


  void FixedUpdate () {

    if(player.health < 1) {
      GetComponent<TextMesh>().text = "GAME OVER";
    } else {
      GetComponent<TextMesh>().text = player.health.ToString();
    }
  }
}
