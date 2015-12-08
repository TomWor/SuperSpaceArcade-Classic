using UnityEngine;
using System.Collections;

public class MenuController : SceneController
{
    public Color trackColor;

    protected static TrackRider sceneCamera;
    public static TrackRider SceneCamera
    {
        get { return sceneCamera; }
        set
        {
            MenuController.sceneCamera = value;
            EventManager.PlayerSpawned(value);
        }
    }

    public void Start()
    {
        GameObject TrackGeneratorObject = Instantiate(Resources.Load("TrackGenerator", typeof(GameObject))) as GameObject;

        MenuController.TrackGenerator = TrackGeneratorObject.GetComponent<TrackGenerator>();
        MenuController.SceneCamera = Camera.main.GetComponent<MenuCamera>();

        EventManager.PlayerSpawned(MenuController.SceneCamera);

        SceneController.currentTrackBorderColor = this.trackColor;
        EventManager.TrackBorderColorChanged(SceneController.currentTrackBorderColor);
    }

}
