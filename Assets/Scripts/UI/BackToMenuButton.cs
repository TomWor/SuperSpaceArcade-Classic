using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SuperSpaceArcade
{

	public class BackToMenuButton : MonoBehaviour
	{

		public void BackToMenu()
		{
			EventManager.MenuEnter();
		}

	}

}
