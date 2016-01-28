using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
	public delegate void PlayerSpawnHandler(TrackRider player);

	public static event PlayerSpawnHandler onPlayerSpawned;

	public delegate void PlayerAddPointsHandler(int points,Vector3 sourcePosition,Quaternion sourceRotation);

	public static event PlayerAddPointsHandler onPlayerAddPoints;

	public delegate void PlayerInvulnerableHandler(bool invulnerable);

	public static event PlayerInvulnerableHandler onPlayerInvulnerable;

	public delegate void TrackBorderColorChangedHandler(Color color);

	public static event TrackBorderColorChangedHandler onTrackBorderColorChanged;


	public static void PlayerSpawned(TrackRider player)
	{
		if (onPlayerSpawned != null)
			onPlayerSpawned(player);
	}


	public static void PlayerAddPoints(int points, Vector3 sourcePosition, Quaternion sourceRotation)
	{
		if (onPlayerAddPoints != null)
			onPlayerAddPoints(points, sourcePosition, sourceRotation);
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
}
