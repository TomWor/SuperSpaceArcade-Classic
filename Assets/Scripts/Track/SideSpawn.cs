using UnityEngine;
using System.Collections;
using PathologicalGames;

public class SideSpawn : MonoBehaviour
{
	// Length of the SideSpawn anchor
	public int anchorLength = 50;

	// Offsets for how far the SideSpawn reaches out forwards
	// or backwards
	public int forwardOffset = 50;
	public int backwardOffset = 0;

	public int likelyhood = 5;

	// Left or right SideSpawn? 0 = left, 1 = right
	public int spawnType = 0;

	// Cached Transform
	private Transform cachedTransform;

	[HideInInspector]
	public string poolName = "Track";

    // vertex color components of the mesh items
    private ChangeVertexColor[] vertexColorComponents;


	public void Awake()
	{
		this.cachedTransform = this.transform;
        this.vertexColorComponents = this.GetComponentsInChildren<ChangeVertexColor>();
	}


    public void OnEnable()
    {
        EventManager.onTrackBorderColorChanged += this.OnTrackBorderColorChanged;
    }


    public void OnDisable()
    {
        EventManager.onTrackBorderColorChanged -= this.OnTrackBorderColorChanged;
    }


    public void OnTrackBorderColorChanged(Color color)
    {
        foreach (ChangeVertexColor vertexColorComponent in this.vertexColorComponents)
        {
            vertexColorComponent.ChangeColor("TrackBorder", color);
        }
    }


    public void OnSpawned()
    {
        foreach (ChangeVertexColor vertexColorComponent in this.vertexColorComponents)
        {
            this.OnTrackBorderColorChanged(GameController.currentTrackBorderColor);
        }
    }
    

	public void Despawn()
	{
		PoolManager.Pools[this.poolName].Despawn( this.cachedTransform );
	}
}
