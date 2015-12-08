using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

    public static Color currentTrackBorderColor;

    protected static TrackGenerator trackGenerator;
    public static TrackGenerator TrackGenerator
    {
        get { return trackGenerator; }
        set
        {
            SceneController.trackGenerator = value;
        }
    }

    protected static Color trackBorderColor;
    public static Color TrackBorderColor
    {
      get { return trackBorderColor; }
      set {
        if( SceneController.trackBorderColor != value )
        {
            SceneController.trackBorderColor = value;
            EventManager.TrackBorderColorChanged(value);
        }
      }
    }

    public IEnumerator ChooseRandomColor()
    {
        while(true){
            SceneController.currentTrackBorderColor = new Color(Random.value, Random.value, Random.value);
            EventManager.TrackBorderColorChanged(SceneController.currentTrackBorderColor);
            yield return new WaitForSeconds(5.0f);
        }
    }
}
