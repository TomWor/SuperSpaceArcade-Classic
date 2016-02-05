using UnityEngine;
using System.Collections;

public class CreditsButton : MonoBehaviour
{
	GameObject mainMenu, creditsScreen;

	public void Awake()
	{
		this.mainMenu = GameObject.Find("MainMenu");
		this.creditsScreen = GameObject.Find("Credits");
	}


	public void DisplayCredits()
	{
		this.mainMenu.SetActive(false);
		this.creditsScreen.SetActive(true);
	}


	public void HideCredits()
	{
		this.mainMenu.SetActive(true);
		this.creditsScreen.SetActive(false);
	}

}
