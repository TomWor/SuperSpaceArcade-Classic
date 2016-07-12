using UnityEngine;
using System.Collections;

namespace SuperSpaceArcade
{
	public class PlayerFollowCamera : MonoBehaviour
	{
		public TrackSpectator player;

		private Quaternion originalRotation;
		private Vector3 originalPosition;


		public void Awake()
		{
			this.originalRotation = this.transform.rotation;
			this.originalPosition = this.transform.position;
		}

		public void OnEnable()
		{
			EventManager.onPlayerSpawned += this.OnPlayerSpawned;
			EventManager.onPlayerDestroyed += this.OnPlayerDestroyed;
			EventManager.onMenuEnter += this.OnMenuEnter;
		}


		public void OnDisable()
		{
			EventManager.onPlayerSpawned -= this.OnPlayerSpawned;
			EventManager.onPlayerDestroyed -= this.OnPlayerDestroyed;
			EventManager.onMenuEnter -= this.OnMenuEnter;
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


		public void OnMenuEnter()
		{
			this.transform.rotation = this.originalRotation;
			this.transform.position = this.originalPosition;
		}

	}
}