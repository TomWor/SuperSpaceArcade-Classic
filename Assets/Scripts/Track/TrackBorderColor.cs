using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SuperSpaceArcade
{
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

			// Revert to 
			foreach (ChangeVertexColor vertexColorComponent in this.vertexColorComponents) {
				vertexColorComponent.ResetItemColor("TrackBorder");
			}
		}


		public void OnTrackBorderColorChanged(Color color)
		{
			foreach (ChangeVertexColor vertexColorComponent in this.vertexColorComponents) {
				vertexColorComponent.ChangeColor("TrackBorder", color);
				//Debug.Log("OnTrackBorderColorChanged: " + color);
			}
		}


		public void OnSpawned()
		{
			foreach (ChangeVertexColor vertexColorComponent in this.vertexColorComponents) {
				//Debug.Log("Setting " + this.gameObject.name + " border to color: " + GameController.currentTrackBorderColor);
				vertexColorComponent.SetItemColor("TrackBorder", GameController.currentTrackBorderColor);
			}
		}
	}
}