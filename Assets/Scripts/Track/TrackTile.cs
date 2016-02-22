using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

namespace SuperSpaceArcade
{
	[AddComponentMenu("BitMonsterFlux/Track/TrackTile")]

	public class TrackTile : MonoBehaviour
	{
		/**
    * Lenght of the individual track tile
    */
		public int trackTileLength = 0;


		/**
    * Vertical offset for next tile
    */
		public int verticalOffset = 0;


		/**
    * Horizontal offset for next tile
    */
		public int horizontalOffset = 0;


		// Connector id for last trackTile
		public int inConnector;

		// The spawnPool this tile is spawned in
		[HideInInspector]
		public string poolName = "Track";

		// poolName For The debris spawns
		[HideInInspector]
		public string debrisPoolName = "Spawns";

		// The tiles 'stressLevel' indicates which difficulty level the game has
		// to reach before this particular tile might get shown
		public int stressLevel = 0;

		// Array of spawnPoints on this tracktile
		private SpawnPoint[] spawnPoints;

		// Array of sideSpawnPoints on this tracktile
		private SideSpawnPoint[] sideSpawnPoints;

		// Connector id for next trackTile
		public int outConnector;

		// Chance of appearing on track
		public int likelyhood = 5;

		// Cached transform
		private Transform cachedTransform;


		public void Awake()
		{
			this.cachedTransform = this.transform;
		}


		/**
    * Cache properties
    */
		public void OnSpawned()
		{
			this.sideSpawnPoints = this.cachedTransform.GetComponentsInChildren<SideSpawnPoint>();

			foreach (SideSpawnPoint sideSpawnPoint in this.sideSpawnPoints) {
				sideSpawnPoint.Spawn();
			}

			this.spawnPoints = this.cachedTransform.GetComponentsInChildren<SpawnPoint>();

			foreach (SpawnPoint spawnPoint in this.spawnPoints) {
				//Debug.Log ("Spawnpoint position: " + spawnPoint.transform.position.ToString ());
				spawnPoint.Spawn();
			}
		}


		/**
    * Despawns this tile form track spawnPool
    */
		public void Despawn()
		{
			foreach (SpawnPoint spawnPoint in this.spawnPoints) {
				spawnPoint.Despawn();
			}

			foreach (SideSpawnPoint sideSpawnPoint in this.sideSpawnPoints) {
				sideSpawnPoint.Despawn();
			}

			// Despawn debris
			Debris[] debris = this.cachedTransform.GetComponentsInChildren<Debris>();
			foreach (Debris d in debris) {
				PoolManager.Pools[this.debrisPoolName].Despawn(d.gameObject.transform, PoolManager.Pools[this.poolName].transform);
			}

			PoolManager.Pools[this.poolName].Despawn(this.cachedTransform);
		}
	}
}