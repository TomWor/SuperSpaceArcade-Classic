using System.ComponentModel;
using UnityEngine;

public partial class SROptions {

	[Category("Utilities")]
	public void ClearPlayerPrefs() {
		Debug.Log("Clearing PlayerPrefs");
		PlayerPrefs.DeleteAll();
	}

	[Category("Cheats")]
	public bool Invulnerability {
		get {
            GameObject player = GameObject.FindWithTag("Player");
						if (player) {
	            return player.GetComponent<Player>().invulnerable;
						}
            return false;
        }
		set {
            GameObject player = GameObject.FindWithTag("Player");
						if (player) {
	            player.GetComponent<Player>().invulnerable = !player.GetComponent<Player>().invulnerable;
						}
		}
	}
}
