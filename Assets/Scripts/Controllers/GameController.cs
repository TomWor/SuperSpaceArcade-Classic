using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SuperSpaceArcade
{
	public class GameController : MonoBehaviour
	{
		public int targetFrameRate = 60;
		public Color[] stressLevelColors;

		public static GameObject mainMenu, creditsScreen, gameOverUI, menuLogo, inGameUI;
		public static GameObject mainCamera;

		private static int currentStressLevel = 0;

		public static int CurrentStressLevel {
			get { return GameController.currentStressLevel; }
			set {
				GameController.currentStressLevel = value;
			}
		}

		public static Color currentTrackBorderColor;


		protected static TrackGenerator trackGenerator;

		public static TrackGenerator TrackGenerator {
			get { return trackGenerator; }
			set {
				GameController.trackGenerator = value;
			}
		}


		protected static Color trackBorderColor;

		public static Color TrackBorderColor {
			get { return trackBorderColor; }
			set {
				if (GameController.trackBorderColor != value) {
					GameController.trackBorderColor = value;
					EventManager.TrackBorderColorChanged(value);
				}
			}
		}


		private static Player player;

		public static Player Player {
			get { return player; }
			set {
				GameController.player = value;
				EventManager.PlayerSpawned(value);
			}
		}


		public static bool playerInvulnerable;

		public static bool PlayerInvulnerable {
			get { return playerInvulnerable; }
			set { 
				GameController.playerInvulnerable = value;
				EventManager.PlayerInvulnerable(value);
			}
		}


		public void Start()
		{
			Application.targetFrameRate = this.targetFrameRate;

			SRDebug.Instance.PanelVisibilityChanged += visible => {
				if (visible) {
					GameController.PauseGame();
				} else {
					GameController.UnPauseGame();
				}
			};

			GameController.mainCamera = Camera.main.gameObject;

			GameController.TrackGenerator = GameObject.FindWithTag("TrackGenerator").GetComponent<TrackGenerator>();
			//GameController.TrackGenerator.

			GameController.mainMenu = GameObject.FindWithTag("MainMenu");
			GameController.mainMenu.SetActive(true);

			GameController.creditsScreen = GameObject.FindWithTag("CreditsScreen");
			GameController.creditsScreen.SetActive(false);

			GameController.gameOverUI = GameObject.FindWithTag("GameOverUI");
			GameController.gameOverUI.SetActive(false);

			GameController.menuLogo = GameObject.FindWithTag("MenuLogo");
			GameController.menuLogo.SetActive(false);

			GameController.inGameUI = GameObject.FindWithTag("InGameUI");
			GameController.inGameUI.SetActive(false);

			this.setInitialStressLevel();
		}


		public void OnEnable()
		{
			EventManager.onGameStart += this.OnGameStart;
			EventManager.onGameOver += this.OnGameOver;
		}


		public void OnDisable()
		{
			EventManager.onGameStart -= this.OnGameStart;
			EventManager.onGameOver -= this.OnGameOver;
		}


		public void OnGameStart()
		{
			GameController.mainMenu.SetActive(false);
			GameController.mainCamera.GetComponent<MenuCamera>().enabled = false;

			GameObject PlayerObject = Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
			GameController.Player = PlayerObject.GetComponent<Player>();
			GameController.Player.TrackGenerator = GameController.TrackGenerator;
			GameController.TrackGenerator.ResetTrack();

			EventManager.PlayerSpawned(GameController.Player);

			this.setInitialStressLevel();
			StartCoroutine(this.UpdateStressLevel());
		}


		public void OnGameOver()
		{
			GameController.gameOverUI.SetActive(true);
			GameController.gameOverUI.transform.Find("ScorePanel/Score").GetComponent<Text>().text = GameController.Player.points.ToString();
		}


		public void setInitialStressLevel()
		{
			GameController.currentStressLevel = 0;
			GameController.currentTrackBorderColor = this.stressLevelColors[GameController.currentStressLevel];
		}


		public IEnumerator UpdateStressLevel()
		{
			while (true) {
				yield return new WaitForSeconds(20.0f);

				GameController.currentStressLevel++;
				//Debug.Log("Entering Level: " + GameController.currentStressLevel);

				if (this.stressLevelColors.Length > GameController.currentStressLevel) {
					GameController.currentTrackBorderColor = this.stressLevelColors[GameController.currentStressLevel];
					EventManager.TrackBorderColorChanged(GameController.currentTrackBorderColor);
				}
			}
		}


		// Pause the game
		public static void PauseGame()
		{
			Time.timeScale = 0.0f;
		}


		// Unpause the game
		public static void UnPauseGame()
		{
			Time.timeScale = 1.0f;
		}


		// Check if game is paused
		public static bool isPaused()
		{
			return (int)Time.timeScale == (int)0.0f;
		}


		public IEnumerator ChooseRandomColor()
		{
			while (true) {
				GameController.currentTrackBorderColor = new Color(Random.value, Random.value, Random.value);
				EventManager.TrackBorderColorChanged(GameController.currentTrackBorderColor);
				yield return new WaitForSeconds(5.0f);
			}
		}
	}
}