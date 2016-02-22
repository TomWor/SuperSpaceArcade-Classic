using UnityEngine;
using System.Collections;

namespace SuperSpaceArcade
{
	public class MenuCamera : TrackSpectator
	{
		public float speed = 5.0f;

		private Vector3 originalPosition;


		void Awake()
		{
			this.originalPosition = this.transform.position;
		}

		void Update()
		{
			float trackVertOffset = this.trackGenerator.CurrentTrackTileVerticalOffset;
			float trackHoriOffset = this.trackGenerator.CurrentTrackTileHorizontalOffset;

			Vector3 moveDirection = new Vector3(this.originalPosition.x + trackHoriOffset, this.originalPosition.y + trackVertOffset, this.transform.position.z + speed);

			this.transform.position = Vector3.Lerp(this.transform.position, moveDirection, Time.deltaTime);
		}

	}
}