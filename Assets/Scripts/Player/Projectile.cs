using UnityEngine;
using System.Collections;
using PathologicalGames;

public class Projectile : MonoBehaviour
{
	public int speed = 100;
	public int lifetime = 2;
	public int damage = 1;

    private Transform cachedTransform;
	private string poolName = "Shots";


	public void Awake()
	{
		this.cachedTransform = this.transform;
	}


	public void OnSpawned()
	{
		TrackGenerator.onTrackReset += this.onTrackReset;
		PoolManager.Pools[this.poolName].Despawn( this.cachedTransform, this.lifetime );
		StartCoroutine( Fly() );
	}


	public void OnDespawned()
	{
		TrackGenerator.onTrackReset -= this.onTrackReset;
	}


	public void OnTriggerEnter( Collider other )
	{
		if ( other.tag == "Enemy" )
		{
			other.SendMessage( "ApplyDamage", this.damage, SendMessageOptions.DontRequireReceiver );
			PoolManager.Pools[this.poolName].Despawn( this.cachedTransform );
		}

		if ( other.tag == "Level" || other.tag == "Obstacle") {
			other.SendMessage( "ApplyDamage", this.damage, SendMessageOptions.DontRequireReceiver );
			PoolManager.Pools[this.poolName].Despawn( this.cachedTransform );
		}
	}


	public void onTrackReset( int trackResetZ )
	{
        this.cachedTransform.position += new Vector3(0, 0, trackResetZ);
	}


	private IEnumerator Fly()
	{
		while ( true )
		{
			this.cachedTransform.Translate( 0, 0, this.speed * Time.deltaTime );
			yield return null;
		}
	}

}
