using UnityEngine;
using System.Collections;

namespace SuperSpaceArcade
{

	public class TrackSpectator : MonoBehaviour
	{

		protected TrackGenerator trackGenerator;

		public TrackGenerator TrackGenerator {
			get {
				return this.trackGenerator;
			}
			set {
				this.trackGenerator = value;
			}
		}

		public void OnEnable()
		{
			TrackGenerator.onTrackCreated += this.OnTrackCreated;
		}


		public virtual void OnTrackCreated(TrackGenerator trackGenerator)
		{
			this.trackGenerator = trackGenerator;
		}

	}
			
}