using UnityEngine;
using System.Collections;

namespace SuperSpaceArcade {

	public class BackToMenuButton : MonoBehaviour {

		public void BackToMenu ()
		{
			Application.LoadLevel("Menu");
		}

	}

}
