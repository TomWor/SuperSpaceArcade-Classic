using UnityEngine;
using System.Collections;

namespace SuperSpaceArcade
{
	public class PlayerFollowCamera : MonoBehaviour
	{
		public TrackSpectator player;

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
			this.GetComponent<SmoothFollow>().enabled = true;
			this.GetComponent<SmoothFollow>().target = this.player.transform.FindChild("ViewTarget").GetComponent<Transform>();
		}

		public void OnPlayerDestroyed()
		{
			this.player = null;
			this.GetComponent<SmoothFollow>().enabled = false;
			this.GetComponent<SmoothFollow>().target = null;
		}

	}
}