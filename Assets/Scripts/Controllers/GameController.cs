using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SuperSpaceArcade
{
	public class GameController : MonoBehaviour
	{
		public int targetFrameRate = 60;
		public Color[] stressLevelColors;
		public GameObject playerObject;

		public GameObject mainMenu, creditsScreen, gameOverUI, menuLogo, inGameUI;
		public GameObject mainCamera;

		private static int currentStressLevel = 0;

		public static int CurrentStressLevel {
			get { return GameController.currentStressLevel; }
			set {
				GameController.currentStressLevel = value;
			}
		}

		public static Color currentTrackBorderColor;


		private TrackGenerator trackGenerator;

		public TrackGenerator TrackGenerator {
			get { return trackGenerator; }
			set {
				this.trackGenerator = value;
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


		private Player player;

		public Player Player {
			get { return player; }
			set {
				this.player = value;
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

			this.mainCamera = Camera.main.gameObject;
			this.TrackGenerator = GameObject.FindWithTag("TrackGenerator").GetComponent<TrackGenerator>();

			this.mainMenu = GameObject.FindWithTag("MainMenu");
			this.mainMenu.SetActive(true);

			this.creditsScreen = GameObject.FindWithTag("CreditsScreen");
			this.creditsScreen.SetActive(false);

			this.gameOverUI = GameObject.FindWithTag("GameOverUI");
			this.gameOverUI.SetActive(false);

			this.menuLogo = GameObject.FindWithTag("MenuLogo");
			this.menuLogo.SetActive(false);

			this.inGameUI = GameObject.FindWithTag("InGameUI");
			this.inGameUI.SetActive(false);

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

			this.gameOverUI.SetActive(false);
			this.mainMenu.SetActive(false);
			this.mainCamera.GetComponent<MenuCamera>().enabled = false;

			this.playerObject = Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
			this.Player = this.playerObject.GetComponent<Player>();
			this.Player.TrackGenerator = this.TrackGenerator;
			this.TrackGenerator.ResetTrack();

			EventManager.PlayerSpawned(this.Player);

			this.setInitialStressLevel();
			StartCoroutine(this.UpdateStressLevel());
		}


		public void OnGameOver()
		{
			this.gameOverUI.SetActive(true);
			this.gameOverUI.transform.Find("ScorePanel/Score").GetComponent<Text>().text = this.Player.points.ToString();
			Destroy(this.playerObject);
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