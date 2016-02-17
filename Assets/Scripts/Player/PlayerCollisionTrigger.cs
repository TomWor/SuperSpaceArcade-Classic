using UnityEngine;
using System.Collections;

public class PlayerCollisionTrigger : MonoBehaviour
{
	public Player player;

	public void OnCollisionEnter(Collision collision)
	{
		if (!this.player.invulnerable) {
			//Debug.Log("Trigger with layer: " + collision.collider.gameObject.layer);
			if (collision.collider.gameObject.layer == 10 || collision.collider.gameObject.layer == 14) {
				this.player.GameOver();
			}
		}
	}

}
