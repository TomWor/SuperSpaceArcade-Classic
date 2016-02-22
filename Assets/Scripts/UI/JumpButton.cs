using UnityEngine;
using System.Collections;

namespace SuperSpaceArcade
{

	public class JumpButton : MonoBehaviour
	{
		private TrackSpectator player;
		private ShipController shipController;

		public void Jump()
		{
			if (this.shipController) {
				this.shipController.Jump();
			}
		}


		public void OnEnable()
		{
			EventManager.onPlayerSpawned += this.OnPlayerSpawned;
		}


		public void OnDisable()
		{
			EventManager.onPlayerSpawned -= this.OnPlayerSpawned;
		}


		public void OnPlayerSpawned(TrackSpectator player)
		{
			this.player = player;
			this.shipController = this.player.GetComponent<ShipController>();
		}

	}

}