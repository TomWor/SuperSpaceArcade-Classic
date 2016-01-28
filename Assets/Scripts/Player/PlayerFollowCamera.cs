using UnityEngine;
using System.Collections;

public class PlayerFollowCamera : MonoBehaviour
{
	public TrackRider player;

	public void OnEnable()
	{
		EventManager.onPlayerSpawned += this.OnPlayerSpawned;
	}


	public void OnDisable()
	{
		EventManager.onPlayerSpawned -= this.OnPlayerSpawned;
	}


	public void OnPlayerSpawned(TrackRider player)
	{
		this.player = player;
		this.GetComponent<SmoothFollow>().target = this.player.transform.FindChild("ViewTarget").GetComponent<Transform>();
	}

}
