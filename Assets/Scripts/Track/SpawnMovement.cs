using UnityEngine;
using System.Collections;
using PathologicalGames;

public class SpawnMovement : MonoBehaviour
{
	public float speed = 12.0f;
  public float range = 20.0f;

  protected Transform cachedTransform;
	//private string poolName = "Spawns";


  public void Awake(){
		this.cachedTransform = this.transform;
	}


  public void OnSpawned()
	{
		foreach (Transform child in transform)
		{
		  child.gameObject.BroadcastMessage("OnSpawned", true, SendMessageOptions.DontRequireReceiver);
		}

		StartCoroutine( Move() );
	}


	public void OnDespawned()
	{
		foreach (Transform child in transform)
		{
		  child.gameObject.BroadcastMessage("OnDespawned", true, SendMessageOptions.DontRequireReceiver);
		}

		StopAllCoroutines();
	}


	private IEnumerator Move()
	{
    while ( true )
		{
			this.cachedTransform.localPosition = new Vector3( Mathf.PingPong( Time.time * this.speed, this.range*2.0f ) - this.range, this.cachedTransform.localPosition.y, this.cachedTransform.localPosition.z );
			yield return new WaitForFixedUpdate();
		}
	}

}
