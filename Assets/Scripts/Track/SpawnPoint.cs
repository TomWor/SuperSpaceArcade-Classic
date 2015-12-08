using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class SpawnPoint : MonoBehaviour
{
	public bool spawnPowerUps = true;
	public bool spawnEnemies = true;
	public bool spawnObstacles = true;
	public bool spawnDecals = true;

	private Transform cachedTransform;

	private string poolName = "Spawns";
	private Transform spawnedItem;
	private TrackGenerator trackGenerator;


	public void Awake()
	{
		this.cachedTransform = this.transform;
		this.spawnedItem = null;

		this.trackGenerator = GameObject.FindGameObjectWithTag( "TrackGenerator" ).GetComponent<TrackGenerator>();
	}


	public void Spawn()
	{
		List<int> spawnTypes = new List<int>();
		if ( this.spawnPowerUps ) { spawnTypes.Add( 1 ); }
		if ( this.spawnEnemies ) { spawnTypes.Add( 2 ); }
		if ( this.spawnObstacles ) { spawnTypes.Add( 3 ); }
		if ( this.spawnDecals ) { spawnTypes.Add( 4 ); }

		GameObject elementToInstantiate = this.trackGenerator.getSpawnElement( spawnTypes );
		this.spawnedItem = PoolManager.Pools[this.poolName].Spawn( elementToInstantiate.transform, this.cachedTransform.position, Quaternion.identity, this.cachedTransform );
	}


	public void Despawn()
	{
		// Only despawn spawnedItem if it's still in spawned state and thus has not been despawned
		// by e.g. exploding
		if ( this.spawnedItem && PoolManager.Pools[this.poolName].IsSpawned( this.spawnedItem ) )
			PoolManager.Pools[this.poolName].Despawn( this.spawnedItem, PoolManager.Pools[this.poolName].transform );
	}

}
