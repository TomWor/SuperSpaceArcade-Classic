using UnityEngine;
using System.Collections;
using PathologicalGames;

public class Destructible : MonoBehaviour
{
	protected Transform cachedTransform;
	protected Vector3 originalPosition;

	public bool exploded = false;
	public float explosionForce = 15.0f;

	public int health = 1;
	protected int _health = 0;

	public int points = 8;

	protected SpawnPool pool;
	public GameObject explodePrefab;

	// Horrible hack to keep Poolmanagers Broadcastmessage from firing twice
	// Somehow Unitys broadcast message iterates to slowly over the targets
	// Having another item spawned can confuse it
	protected bool isSpawned = false;


	public void ApplyDamage(int damage)
	{
		this._health -= damage;
		if (this._health <= 0) {
			this.Explode();
		} 
	}


	public void Explode()
	{
		// Spawn points prefab
		int playerAddPoints = GameController.playerInvulnerable ? this.points * 2 : this.points;
		EventManager.PlayerAddPoints(playerAddPoints, this.cachedTransform.position, this.cachedTransform.rotation);

		// Check if the enemy is already exploded
		// Prevent multiple explosions triggering
		if (this.exploded == false) {
			TrackTile parentTrackTile = this.cachedTransform.GetComponentInParent<TrackTile>();

			// Only replace the prefab if an explode prefab is provided, otherwise stay, e.g. boxes, energy barriers
			if (this.explodePrefab) {

				Transform explodedEnemy = this.pool.Spawn(explodePrefab.transform, this.cachedTransform.position, Quaternion.identity, parentTrackTile.transform);

				// Catch all debris elements with and Overlapsphere and maybe even other debris not part of the enemy object
				Collider[] colliders = Physics.OverlapSphere(explodedEnemy.transform.position, 20.0f);
				foreach (Collider c in colliders) {
					// If the debris gameobject has no rigidbody component, we can't apply an explosion force to it
					if (c.GetComponent<Rigidbody>() == null)
						continue;

					// Set the explosion position to be slightly in front of the enemy to simulate projectile impact force, pushing
					// the debris forward on the z axis
					Vector3 explosionPosition = this.cachedTransform.position - new Vector3(0, 0, 10.0f);
					c.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(240, 360), explosionPosition, 100.0f, 1.0f, ForceMode.Impulse);
				}

				this.exploded = true;

				// Move enemy object down below the track, so it is out of sight
				// but leave despawn logic to the parent tracktile to not mess with
				// track despawn workflow
				this.transform.position += Vector3.down * 2000;
			}
		}
	}
}
