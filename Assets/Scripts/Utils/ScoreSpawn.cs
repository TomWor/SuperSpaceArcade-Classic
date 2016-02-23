using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

namespace SuperSpaceArcade
{
	public class ScoreSpawn : MonoBehaviour
	{
		private string poolName = "Score";
		private List<GameObject> characterPrefabs;


		// Use this for initialization
		public void Start()
		{
			this.LoadAndPoolAlphabet("Alphabet");

			EventManager.onPlayerAddPoints += this.SpawnPoints;
		}


		private List<GameObject> LoadAndPoolAlphabet(string resourcePath)
		{
			// New temporary list of loaded spawns
			List<GameObject> spawnList = new List<GameObject>();

			// Create Pool if it doesn't exist
			if (!PoolManager.Pools.ContainsKey("Alphabet")) {
				PoolManager.Pools.Create("Alphabet");
			}

			// Loop through all the powerUps and add them to list
			foreach (GameObject g in Resources.LoadAll<GameObject>( resourcePath )) {

				PrefabPool prefabPool = new PrefabPool(g.transform);
				prefabPool.preloadAmount = 5; // g.GetComponent<TrackTile>().likelyhood;
				PoolManager.Pools["Alphabet"].CreatePrefabPool(prefabPool);

				spawnList.Add(g);
			}

			return spawnList;
		}


		// Update is called once per frame
		public void SpawnPoints(int points, Vector3 sourcePosition, Quaternion sourceRotation, Transform target)
		{
			Transform scorePrefab = PoolManager.Pools[this.poolName].Spawn("Score", sourcePosition, Quaternion.identity, target);
			scorePrefab.gameObject.GetComponent<Score>().ScoreValue = points;
		}
	}
}