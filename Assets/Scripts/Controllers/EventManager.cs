using UnityEngine;
using System.Collections;

namespace SuperSpaceArcade
{
	public class EventManager : MonoBehaviour
	{
		public static TrackSpectator player;


		public delegate void GameStartHandler();

		public static event GameStartHandler onGameStart;


		public delegate void PlayerSpawnHandler(TrackSpectator player);

		public static event PlayerSpawnHandler onPlayerSpawned;


		public delegate void PlayerDestroyedHandler();

		public static event PlayerDestroyedHandler onPlayerDestroyed;


		public delegate void PlayerAddPointsHandler(int points,Vector3 sourcePosition,Quaternion sourceRotation,Transform target);

		public static event PlayerAddPointsHandler onPlayerAddPoints;


		public delegate void PlayerInvulnerableHandler(bool invulnerable);

		public static event PlayerInvulnerableHandler onPlayerInvulnerable;


		// Track border color changed, stress level has increased
		public delegate void TrackBorderColorChangedHandler(Color color);

		public static event TrackBorderColorChangedHandler onTrackBorderColorChanged;


		// Track gets created, new game starts
		public delegate void TrackCreatedHandler(TrackGenerator trackGenerator);

		public static event TrackCreatedHandler onTrackCreated;


		// Fired when track is finished building for the first time
		public delegate void TrackReadyHandler();

		public static event TrackReadyHandler onTrackReady;


		// Player ship got destroyed
		public delegate void GameOverHandler();

		public static event GameOverHandler onGameOver;





		public static void GameStart()
		{
			if (onGameStart != null)
				onGameStart();
		}


		public static void PlayerSpawned(TrackSpectator player)
		{
			EventManager.player = player;
			onPlayerInvulnerable = null;

			if (onPlayerSpawned != null)
				onPlayerSpawned(player);
		}


		public static void PlayerDestroyed()
		{
			EventManager.player = null;
			onPlayerInvulnerable = null;

			if (onPlayerDestroyed != null)
				onPlayerDestroyed();
		}


		public static void PlayerAddPoints(int points, Vector3 sourcePosition, Quaternion sourceRotation, Transform target)
		{
			if (onPlayerAddPoints != null)
				onPlayerAddPoints(points, sourcePosition, sourceRotation, player.transform);
		}


		public static void PlayerInvulnerable(bool invulnerable)
		{
			if (onPlayerInvulnerable != null)
				onPlayerInvulnerable(invulnerable);
		}


		public static void TrackBorderColorChanged(Color color)
		{
			if (onTrackBorderColorChanged != null)
				onTrackBorderColorChanged(color);
		}


		/*
	     * Gets called when the Track Component has been created
	     */
		public static void TrackCreated(TrackGenerator trackGenerator)
		{
			if (onTrackCreated != null)
				onTrackCreated(trackGenerator);
		}


		public static void TrackReady()
		{
			if (onTrackReady != null)
				onTrackReady();
		}


		public static void GameOver()
		{
			if (onGameOver != null)
				onGameOver();
		}

	}
}
