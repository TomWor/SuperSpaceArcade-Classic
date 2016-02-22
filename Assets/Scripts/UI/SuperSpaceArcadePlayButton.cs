using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SuperSpaceArcade
{
	public class SuperSpaceArcadePlayButton : MonoBehaviour
	{

		public void EnterGame()
		{
			EventManager.GameStart();
		}
	}
}