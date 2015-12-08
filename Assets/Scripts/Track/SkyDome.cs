using UnityEngine;
using System.Collections;
using PathologicalGames;

public class SkyDome : MonoBehaviour
{

    private TrackRider player;
    private TransformConstraint constraint;

    private Color originalSkyColor;
    private Color targetSkyColor;
    private Color currentSkyColor;

    private Color targetAmbientColor;
    private Color currentAmbientColor;

    public Color skyColor;
    private MeshRenderer cachedMeshRenderer;

    private Light sun;


    public void Awake()
    {
        this.constraint = this.GetComponent<TransformConstraint>();
        this.cachedMeshRenderer = this.GetComponent<MeshRenderer>();
        this.originalSkyColor = this.cachedMeshRenderer.sharedMaterial.color;
        this.currentSkyColor = this.originalSkyColor;
        this.sun = GameObject.FindWithTag("Sun").GetComponent<Light>();
    }


    public void OnEnable()
    {
        EventManager.onPlayerSpawned += this.OnPlayerSpawned;
        //EventManager.onTrackBorderColorChanged += this.OnTrackBorderColorChanged;
    }


    public void OnDisable()
    {
        EventManager.onPlayerSpawned -= this.OnPlayerSpawned;
        //EventManager.onTrackBorderColorChanged -= this.OnTrackBorderColorChanged;
    }


    public void OnPlayerSpawned(TrackRider player)
    {
        this.player = player;
        this.constraint.target = this.player.transform;
    }


    public void FixedUpdate()
    {
        /*
        if ( this.currentSkyColor != this.targetSkyColor ){

            this.currentSkyColor = Color.Lerp(this.currentSkyColor, this.targetSkyColor, Time.deltaTime);
            this.cachedMeshRenderer.sharedMaterial.color = this.currentSkyColor;

            this.currentAmbientColor = Color.Lerp(this.currentAmbientColor, this.targetAmbientColor, Time.deltaTime);
            RenderSettings.ambientLight = this.currentAmbientColor;
        }
        */
    }


    public void OnTrackBorderColorChanged(Color color)
    {
        Color differentColor = new Color(color.g, color.b, color.r);
        HSLColor skyColor = HSLColor.FromRGBA(differentColor);

        skyColor.l = 0.8f;
        this.targetSkyColor = skyColor.ToRGBA();

        skyColor.l = 0.9f;
        this.targetAmbientColor = skyColor.ToRGBA();

        this.sun.color = this.targetAmbientColor;
    }

}
