using UnityEngine;
using System.Collections;
using System;

public class GameController : SceneController
{
    public Color[] stressLevelColors;

    private static int currentStressLevel = 0;
    public static int CurrentStressLevel
    {
        get { return GameController.currentStressLevel; }
        set
        {
            GameController.currentStressLevel = value;
        }
    }

    private static TrackRider player;
    public static TrackRider Player
    {
        get { return player; }
        set
        {
            GameController.player = value;
            EventManager.PlayerSpawned(value);
        }
    }


    public void Start()
    {

        SRDebug.Instance.PanelVisibilityChanged += visible =>
        {
            if (visible)
            {
              GameController.PauseGame();
            } else {
              GameController.UnPauseGame();
            }
        };


        GameObject TrackGeneratorObject = Instantiate(Resources.Load("TrackGenerator", typeof(GameObject))) as GameObject;
        GameController.TrackGenerator = TrackGeneratorObject.GetComponent<TrackGenerator>();

        GameObject PlayerObject = Instantiate(Resources.Load("Player", typeof(GameObject))) as GameObject;
        GameController.Player = PlayerObject.GetComponent<Player>();
        GameController.Player.TrackGenerator = GameController.TrackGenerator;

        EventManager.PlayerSpawned(GameController.Player);

        StartCoroutine(this.UpdateStressLevel());
    }


    public void OnLevelWasLoaded()
    {
        GameController.currentStressLevel = 0;
        SceneController.currentTrackBorderColor = this.stressLevelColors[GameController.currentStressLevel];
    }


    public IEnumerator UpdateStressLevel()
    {
        while(true)
        {
            yield return new WaitForSeconds(10.0f);

            GameController.currentStressLevel++;
            //Debug.Log("Entering Level: "+GameController.currentStressLevel);

            if( this.stressLevelColors.Length > GameController.currentStressLevel )
            {
                SceneController.currentTrackBorderColor = this.stressLevelColors[GameController.currentStressLevel];
                EventManager.TrackBorderColorChanged(SceneController.currentTrackBorderColor);
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
        return Time.timeScale == 0.0f;
    }

}