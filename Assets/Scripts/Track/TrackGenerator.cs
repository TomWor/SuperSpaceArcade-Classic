using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

namespace SuperSpaceArcade
{

	[AddComponentMenu("SuperSpaceArcade/Track/TrackGenerator")]
	public class TrackGenerator : MonoBehaviour
	{
		// The offsets mark the beginning and the end of the track,
		// relative to zero
		public int forwardTrackOffset = 100;
		public int backwardTrackOffset = 20;

		// If players Z index reaches this value, all track tiles and the player
		// position get reset by the same value, snapping everything back relative to zero
		// Is done to avoid float overflow errors if the track gets to long
		public int trackResetZ = 100;

		// Bool gets set to true when initial track is built
		// Fires the OnTrackReady method on the EventManager
		public bool trackReady = false;

		// True if track reset is in progress
		// Every PowerUp / Enemy collider hit needs to check for this static variable,
		// otherwise the track reset might mess with the physics collisions
		// see FixedUpdate() and LateUpdate()
		public static bool trackResetActive = false;

		// These are the current offsets on x and y to mark where the next track tile
		// gets attached
		private int currentHorizontalOffset = 0;
		private int currentVerticalOffset = 0;

		private ulong currentTrackPosition = 0;

		public ulong CurrentTrackPosition {
			get {
				return this.currentTrackPosition;
			}
		}

		// Reference to the player object, set through event listeners in OnEnable()
		// Alters the offset for when new tracktiles get added, see BuildTrack()

		public TrackSpectator player;

		// SpawnPool for track tiles
		private string poolName = "Track";

		// List of TrackTiles
		private List<GameObject> trackTileItems = new List<GameObject>();

		// List of TrackTiles to be used as start items
		private List<GameObject> startTileItems = new List<GameObject>();

		// List of TrackTiles
		private List<GameObject> sideSpawns = new List<GameObject>();

		// All active track tiles are in this list
		private List<TrackTile> activeTrackTiles = new List<TrackTile>();

		public TrackTile currentTrackTile;

		public float CurrentTrackTileVerticalOffset {
			get {
				if (this.currentTrackTile)
					return this.currentTrackTile.transform.position.y;
				return 0.0f;
			}
		}

		public float CurrentTrackTileHorizontalOffset {
			get {
				if (this.currentTrackTile)
					return this.currentTrackTile.transform.position.x;
				return 0.0f;
			}
		}

		// List of available PowerUps
		private List<GameObject> spawns = new List<GameObject>();

		// Event delegate for every track element that needs to be informed of a track reset
		public delegate void TrackResetHandler(int trackResetZ);

		public static event TrackResetHandler onTrackReset;


		public void Start()
		{
			this.startTileItems = this.LoadAndPoolTrackTiles("Track/Start");
			this.trackTileItems = this.LoadAndPoolTrackTiles("Track/Regular");
			this.sideSpawns = this.LoadAndPoolSideSpawns("Track/SideSpawn");

			this.spawns.AddRange(this.LoadAndPoolSpawns("Spawns/PowerUps"));
			this.spawns.AddRange(this.LoadAndPoolSpawns("Spawns/Enemies"));
			this.spawns.AddRange(this.LoadAndPoolSpawns("Spawns/Obstacles"));
			this.spawns.AddRange(this.LoadAndPoolSpawns("Spawns/Decals"));

			this.CreateTrack();
		}


		public void CreateTrack()
		{
			// Stop track building Coroutine if running
			this.StopAllCoroutines();

			this.currentHorizontalOffset = 0;
			this.currentVerticalOffset = 0;
			this.currentTrackPosition = 0;

			foreach (TrackTile trackTile in this.activeTrackTiles) {
				trackTile.Despawn();
			}
			this.activeTrackTiles.Clear();

			// Instantiate random first prefab from startTileItems
			Transform tileToInstantiate;
			tileToInstantiate = startTileItems[Random.Range(0, this.startTileItems.Count)].transform;
			this.currentTrackTile = this.addTrackTile(tileToInstantiate);

			this.StartCoroutine(BuildTrack());
			EventManager.TrackCreated(this);
		}


		private List<GameObject> LoadAndPoolTrackTiles(string tileType)
		{
			// New temporary list of loaded track tiles
			List<GameObject> trackTileList = new List<GameObject>();

			// Loop through all the track tiles and add them to list
			foreach (GameObject g in Resources.LoadAll<GameObject>(tileType)) {

				PrefabPool prefabPool = new PrefabPool(g.transform);
				prefabPool.preloadAmount = g.GetComponent<TrackTile>().likelyhood;
				PoolManager.Pools[this.poolName].CreatePrefabPool(prefabPool);

				trackTileList.Add(g);
			}

			return trackTileList;
		}


		private List<GameObject> LoadAndPoolSideSpawns(string tileType)
		{
			// New temporary list of loaded track tiles
			List<GameObject> trackTileList = new List<GameObject>();

			// Loop through all the track tiles and add them to list
			foreach (GameObject g in Resources.LoadAll<GameObject>(tileType)) {

				PrefabPool prefabPool = new PrefabPool(g.transform);
				prefabPool.preloadAmount = g.GetComponent<SideSpawn>().likelyhood;
				PoolManager.Pools[this.poolName].CreatePrefabPool(prefabPool);

				trackTileList.Add(g);
			}

			return trackTileList;
		}


		private List<GameObject> LoadAndPoolSpawns(string resourcePath)
		{
			// New temporary list of loaded spawns
			List<GameObject> spawnList = new List<GameObject>();

			// Create Pool if it doesn't exist
			if (!PoolManager.Pools.ContainsKey("Spawns")) {
				PoolManager.Pools.Create("Spawns");
			}

			// Loop through all the powerUps and add them to list
			foreach (GameObject g in Resources.LoadAll<GameObject>(resourcePath)) {

				PrefabPool prefabPool = new PrefabPool(g.transform);
				prefabPool.preloadAmount = 5; // g.GetComponent<TrackTile>().likelyhood;
				PoolManager.Pools["Spawns"].CreatePrefabPool(prefabPool);

				spawnList.Add(g);
			}

			return spawnList;
		}


		public GameObject getSpawnElement(List<int> spawnTypes)
		{
			List<GameObject> spawnList = this.spawns.FindAll(p => spawnTypes.Contains(p.GetComponent<Spawn>().spawnType) && p.GetComponent<Spawn>().stressLevel <= GameController.CurrentStressLevel);

			// Prepare a list with all possible Spawns entered as often as their likelyhood value
			List<GameObject> spawnLikelyhood = new List<GameObject>();

			foreach (GameObject spawn in spawnList) {
				for (int x = 1; x <= spawn.GetComponent<Spawn>().likelyhood; x++)
					spawnLikelyhood.Add(spawn);
			}

			GameObject spawnToInstantiate;
			spawnToInstantiate = spawnLikelyhood[Random.Range(0, spawnLikelyhood.Count)];

			return spawnToInstantiate;
		}


		public GameObject getSideSpawnElement(int spawnType, int maxForwardOffset, int maxBackwardOffset, int maxAnchorLength)
		{
			List<GameObject> sideSpawnList = this.sideSpawns.FindAll(p => {
				SideSpawn sideSpawnComponent = p.GetComponent<SideSpawn>();
				return sideSpawnComponent.spawnType == spawnType &&
				sideSpawnComponent.forwardOffset <= maxForwardOffset &&
				sideSpawnComponent.backwardOffset <= maxBackwardOffset &&
				sideSpawnComponent.anchorLength <= maxAnchorLength;
			});

			// Prepare a list with all possible SideSpawns entered as often as their likelyhood value
			List<GameObject> sideSpawnLikelyhood = new List<GameObject>();

			foreach (GameObject sideSpawn in sideSpawnList) {
				for (int x = 1; x <= sideSpawn.GetComponent<SideSpawn>().likelyhood; x++)
					sideSpawnLikelyhood.Add(sideSpawn);
			}

			GameObject sideSpawnToInstantiate;
			sideSpawnToInstantiate = sideSpawnLikelyhood[Random.Range(0, sideSpawnLikelyhood.Count)];

			return sideSpawnToInstantiate;
		}


		public void OnEnable()
		{
			EventManager.onPlayerSpawned += this.OnPlayerSpawned;
			EventManager.onPlayerDestroyed += this.OnPlayerDestroyed;
		}


		public void OnDisable()
		{
			EventManager.onPlayerSpawned -= this.OnPlayerSpawned;
			EventManager.onPlayerDestroyed -= this.OnPlayerDestroyed;
		}


		public void OnPlayerSpawned(TrackSpectator player)
		{
			this.player = player;
			this.player.TrackGenerator = this;
		}

		public void OnPlayerDestroyed()
		{
			this.player = null;
		}


		public void FixedUpdate()
		{
			// Check for track z index reset
			if (this.player) {
				if (this.player.transform.position.z >= this.trackResetZ) {
					// Track reset is in progress
					TrackGenerator.trackResetActive = true;

					// Reset player position
					this.player.transform.position = this.player.transform.position + new Vector3(0, 0, -this.trackResetZ);

					// Reset track tile positions
					foreach (TrackTile tile in this.activeTrackTiles) {
						tile.transform.position = tile.transform.position + new Vector3(0, 0, -this.trackResetZ);
					}

					// Call event to inform all listeners of track reset
					if (onTrackReset != null)
						onTrackReset(-this.trackResetZ);

					// NOTE: TrackGenerator.trackResetActive gets reset in LateUpdate, so it doesn't mess with physics
				}
			}
		}


		public void LateUpdate()
		{

			// Set current TrackTile to measure current falling death offset
			// and camera offset for menu screen
			TrackTile currentTrackTileResult = this.activeTrackTiles.Find(tile => tile.gameObject.transform.position.z < 0 && tile.gameObject.transform.position.z + tile.trackTileLength > 0);
			if (currentTrackTileResult)
				this.currentTrackTile = currentTrackTileResult;

			// Despawn tiles
			List<TrackTile> despawnTiles = this.activeTrackTiles.FindAll(tile => tile.gameObject.transform.position.z < -this.backwardTrackOffset);

			foreach (TrackTile tile in despawnTiles) {
				tile.Despawn();
			}

			// Remove the from trackRecorder too
			this.activeTrackTiles.RemoveAll(tile => tile.gameObject.transform.position.z < -this.backwardTrackOffset);

			// The trackResetActive variable needs to be set back to false here
			// because the Physics calculations with colliders triggering are done directly after
			// FixedUpdate and that led to weird effects on PowerUps being collected when the
			// player was already at a different location
			// This assures that physics are already handled when we reset trackResetActive
			TrackGenerator.trackResetActive = false;
		}


		/// <summary>
		/// Builds the track.
		/// </summary>
		/// <returns>The track.</returns>
		private IEnumerator BuildTrack()
		{
			while (true) {
				// Set the current offset, add player position if available
				float currentOffset = this.forwardTrackOffset;
				if (this.player)
					currentOffset += this.player.transform.position.z;

				TrackTile lastTile = this.activeTrackTiles[this.activeTrackTiles.Count - 1];
				if (lastTile.gameObject.transform.position.z < currentOffset) {
					// Filter list of all tiles, determine possible next tiles by connectors
					List<GameObject> tilesList = this.trackTileItems.FindAll(p => p.GetComponent<TrackTile>().inConnector == lastTile.outConnector && p.GetComponent<TrackTile>().stressLevel <= GameController.CurrentStressLevel);

					// Prepare a list with all possible tiles entered as often as their likelyhood value
					List<GameObject> tilesLikelyhood = new List<GameObject>();

					foreach (GameObject trackTile in tilesList) {
						for (int x = 1; x <= trackTile.GetComponent<TrackTile>().likelyhood; x++)
							tilesLikelyhood.Add(trackTile);
					}

					GameObject tileToInstantiate;
					tileToInstantiate = tilesLikelyhood[Random.Range(0, tilesLikelyhood.Count - 1)];

					// Instantiate TrackTile
					Vector3 lastPosition = lastTile.gameObject.transform.position;
					this.currentHorizontalOffset = this.currentHorizontalOffset + lastTile.horizontalOffset;
					this.currentVerticalOffset = this.currentVerticalOffset + lastTile.verticalOffset;

					Vector3 nextPosition = new Vector3(this.currentHorizontalOffset, this.currentVerticalOffset, lastPosition.z + lastTile.trackTileLength);
					this.addTrackTile(tileToInstantiate.transform, nextPosition);

				} else {

					if (!this.trackReady) {
						this.trackReady = true;
						EventManager.TrackReady();
					}
				}

				yield return new WaitForEndOfFrame();
			}
		}


		/// Add a track tile
		/// <summary>
		/// Adds a track tile.
		/// </summary>
		/// <param name="tileToInstantiate">Tile to instantiate.</param>
		/// <param name="position">Position.</param>/
		private TrackTile addTrackTile(Transform tileToInstantiate, Vector3 position = default(Vector3))
		{
			Transform trackTileTransform = PoolManager.Pools[this.poolName].Spawn(tileToInstantiate, position, Quaternion.identity);

			//Debug.Log ("Track Tile " + trackTileTransform.gameObject.ToString () + " " + trackTileTransform.position.ToString ());

			if (trackTileTransform) {

				// Cache trackTile GetComponent() call
				TrackTile trackTile = trackTileTransform.GetComponent<TrackTile>();
				this.activeTrackTiles.Add(trackTile);

				this.currentTrackPosition += (ulong)trackTile.trackTileLength;

				return trackTile;
			}

			return new TrackTile();
		}

	}
}