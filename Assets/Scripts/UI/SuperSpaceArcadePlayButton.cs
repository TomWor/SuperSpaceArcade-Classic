using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SuperSpaceArcadePlayButton : MonoBehaviour
{

	public void EnterGame()
	{
		SceneManager.LoadScene("Game");
	}
}
