using UnityEngine;
using System.Collections;
using PathologicalGames;

namespace SuperSpaceArcade
{
	/*
	 * Base PowerUp class for all collectable PowerUps
	 */
	public class PowerUp : MonoBehaviour
	{
		public GameObject effectPrefab;

		public int addPoints = 2;

		public string sendMessage = "PowerUpPoints";

		private bool collected = false;

		private string poolName = "Effects";

		public string PoolName {
			get { return this.poolName; }
		}


		void OnDespawned()
		{
			this.GetComponentInChildren<MeshRenderer>().enabled = true;
			this.collected = false;
		}


		void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player" && !TrackGenerator.trackResetActive) {
				this.OnCollect(other);
			}

		}


		public void OnCollect(Collider other)
		{
			if (this.collected)
				return;

			//Debug.Log ( "Collected " + this.ToString () + " by " + other.gameObject.ToString () + " at " + other.transform.position.ToString () );
			//Debug.Log ( "TrackGenerator reset active: " + TrackGenerator.trackResetActive.ToString () );
			other.SendMessage(this.sendMessage, this.addPoints, SendMessageOptions.DontRequireReceiver);

			this.collected = true;
			this.GetComponentInChildren<MeshRenderer>().enabled = false;

			// Spawn effect prefab
			PoolManager.Pools[this.poolName].Spawn(this.effectPrefab.transform, other.transform.position, Quaternion.identity, other.transform);
		}

	}
}