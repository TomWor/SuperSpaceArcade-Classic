using UnityEngine;
using System.Collections;
using PathologicalGames;

public class Enemy : MonoBehaviour
{
	public GameObject explodePrefab;

	protected Transform cachedTransform;
	protected MeshRenderer cachedMeshRenderer;
	protected Vector3 originalPosition;

	public int health = 1;
	private int _health = 0;

	public Color hitColor;
	private Color originalColor;

	public float explosionForce = 15.0f;
	public int points = 8;

	public bool exploded = false;
	protected SpawnPool pool;

	// Horrible hack to keep Poolmanagers Broadcastmessage from firing twice
	// Somehow Unitys broadcast message iterates to slowly over the targets
	// Having another item spawned can confuse it
	protected bool isSpawned = false;


	public void Awake()
	{
		this.cachedMeshRenderer = this.GetComponentInChildren<MeshRenderer>();
		this.cachedTransform = this.transform;

		this.originalPosition = this.cachedTransform.localPosition;
		this.ResetDefaultValues();
	}


	public void OnSpawned()
	{
		if ( !this.isSpawned )
		{
			this.pool = PoolManager.Pools["Spawns"];
		}

		this.isSpawned = true;
	}


	public void OnDespawned()
	{
		//Debug.Log( "OnDespawned: " + this.gameObject.ToString() );
		this.ResetDefaultValues();
	}


	//
	public void Explode()
	{
		// Spawn points prefab
		// TODO: make generic
		//PoolManager.Pools["Points"].Spawn( "128", this.cachedTransform.position, Quaternion.identity, this.cachedTransform.parent );
		EventManager.PlayerAddPoints( this.points, this.cachedTransform.position, this.cachedTransform.rotation );

		// Check if the enemy is already exploded
		// Prevent multiple explosions triggering
		if ( this.exploded == false )
		{
      TrackTile parentTrackTile = this.cachedTransform.GetComponentInParent<TrackTile>();

			if ( this.explodePrefab )
			{
	      Transform explodedEnemy = this.pool.Spawn( explodePrefab.transform, this.cachedTransform.position, Quaternion.identity, parentTrackTile.transform );

				// Catch all debris elements with and Overlapsphere and maybe even other debris not part of the enemy object
				Collider[] colliders = Physics.OverlapSphere( explodedEnemy.transform.position, 20.0f );
				foreach ( Collider c in colliders )
				{
					// If the debris gameobject has no rigidbody component, we can't apply an explosion force to it
					if ( c.GetComponent<Rigidbody>() == null )
						continue;

					// Set the explosion position to be slightly in front of the enemy to simulate projectile impact force, pushing
					// the debris forward on the z axis
					Vector3 explosionPosition = this.cachedTransform.position - new Vector3( 0, 0, 10.0f );
					c.GetComponent<Rigidbody>().AddExplosionForce( Random.Range( 240, 360 ), explosionPosition, 100.0f, 1.0f, ForceMode.Impulse );
				}

				this.exploded = true;

			}

			// Move enemy object down below the track, so it is out of sight
			// but leave despawn logic to the parent tracktile to not mess with
			// track despawn workflow
			this.transform.position += Vector3.down * 2000;
		}
	}


	public void ApplyDamage( int damage )
	{
		//Debug.Log( "Apply Damage. Current health: " + this._health + " exploded: " + this.exploded.ToString() );
		this._health -= damage;
		if ( this._health <= 0 )
		{
			this.Explode();
		}
		else
		{
			// TODO: Color the enemy different
		}
	}


	public void ResetDefaultValues()
	{
        this.transform.localPosition = this.originalPosition;
        this._health = this.health;
		this.exploded = false;
		this.isSpawned = false;

		// Get all children with the SaveRestoreTransform component
		SaveRestoreTransform[] children = this.transform.GetComponentsInChildren<SaveRestoreTransform> ();

        // Call restore method on every child
        foreach ( SaveRestoreTransform child in children )
		{
			child.RestoreTransform ();
		}
	}


	public void OnTriggerEnter ( Collider other)
	{
        if (!TrackGenerator.trackResetActive)
        {
            if (other.gameObject.tag == "Player")
            {
                this.Explode();
                other.SendMessage("Collision", 100, SendMessageOptions.DontRequireReceiver);
            }
        }

    }

}
