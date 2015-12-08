using UnityEngine;
using System.Collections;
using PathologicalGames;

/*
 * Base PowerUp class for all collectable PowerUps
 */
public class PowerUp : MonoBehaviour
{
	public GameObject effectPrefab;

	public int addPoints = 2;

	public string sendMessage = "PowerUpPoints";

	private bool collected = false;

	private Transform cachedTransform;
	//private string poolName = "Spawns";
	private string effectsPoolName = "Effects";


	/*
	* Awake
	* Gets reference to Player GameObject
	*/
	void OnSpawned()
	{
		//this.cachedTransform = this.transform;
	}


	void OnDespawned()
	{
		this.GetComponentInChildren<MeshRenderer>().enabled = true;
		this.collected = false;
	}


	/*
	* OnTriggerEnter
	*/
	void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Player" && !TrackGenerator.trackResetActive )
		{
			this.OnCollect( other );
			//PoolManager.Pools[this.poolName].Despawn( this.cachedTransform );
		}

	}


	public void OnCollect( Collider other )
	{
		if ( this.collected )
			return;

		//Debug.Log ( "Collected " + this.ToString () + " by " + other.gameObject.ToString () + " at " + other.transform.position.ToString () );
		//Debug.Log ( "TrackGenerator reset active: " + TrackGenerator.trackResetActive.ToString () );
		other.SendMessage( this.sendMessage, this.addPoints, SendMessageOptions.DontRequireReceiver );

		this.collected = true;
		this.GetComponentInChildren<MeshRenderer>().enabled = false;

		// Spawn effect prefab
		PoolManager.Pools[this.effectsPoolName].Spawn( this.effectPrefab.transform, other.transform.position, Quaternion.identity, other.transform );
	}

}
