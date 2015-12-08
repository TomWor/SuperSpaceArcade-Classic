using UnityEngine;
using System.Collections;

public class SaveRestoreTransform : MonoBehaviour
{

		private Vector3 localPosition;
		private Quaternion localRotation;

		public void Awake ()
		{
				this.SaveTransform ();
		}

		public void SaveTransform ()
		{
				this.localPosition = this.transform.localPosition;
				this.localRotation = this.transform.localRotation;
		}

		public void RestoreTransform ()
		{
				//Debug.Log ("Restore position " + this.transform.parent.gameObject.ToString ());
				//Debug.Log ("Old position: " + this.transform.position.ToString ());
				this.transform.localPosition = this.localPosition;
				this.transform.localRotation = this.localRotation;
				//Debug.Log ("New position: " + this.transform.position.ToString ());
		}

}
