using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackBorderColor : MonoBehaviour
{
    // vertex color components of the mesh items
    private ChangeVertexColor[] vertexColorComponents;


	public void Awake()
	{
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
}

