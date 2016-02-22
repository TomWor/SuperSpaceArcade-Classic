using UnityEngine;
using System.Collections;
using PathologicalGames;

namespace SuperSpaceArcade
{
	public class SideSpawnPoint : MonoBehaviour
	{
		[HideInInspector]
		public string poolName = "Track";

		// Maximum length of the anchor on the Track,
		// the part where the SideSpawn can touch/connect with the Track
		public int maxAnchorLength = 50;

		// Left or right side of the TrackTile? 0 = left, 1 = right
		public int spawnType = 0;

		// Offsets for how far the SideSpawn might reach out
		// forwards or backwards relative to its anchor on the TrackTile
		public int maxForwardOffset = 50;
		public int maxBackwardOffset = 0;

		// Cached Transform
		private Transform cachedTransform;

		// Reference to the TrackGenerator
		private TrackGenerator trackGenerator;

		private Transform spawnedItem;


		//
		// Awake
		//
		public void Awake()
		{
			this.cachedTransform = this.transform;
			this.trackGenerator = GameObject.FindGameObjectWithTag("TrackGenerator").GetComponent<TrackGenerator>();
		}


		//
		// Spawn the sideSpawn
		//
		public void Spawn()
		{
			GameObject elementToInstantiate = this.trackGenerator.getSideSpawnElement(this.spawnType, this.maxForwardOffset, this.maxBackwardOffset, this.maxAnchorLength);
			this.spawnedItem = PoolManager.Pools[this.poolName].Spawn(elementToInstantiate.transform, this.cachedTransform.position, Quaternion.identity, this.cachedTransform);
		}


		//
		// Despawn the sideSpawn
		//
		public void Despawn()
		{
			PoolManager.Pools[this.poolName].Despawn(this.spawnedItem, PoolManager.Pools[this.poolName].transform);
		}


	}
}