using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SuperSpaceArcade;


public class ChangeVertexColor : MonoBehaviour
{
	public VertexColorItem[] colorItems;

	private MeshFilter cachedMeshFilter;
	private Color[] meshColors;
	private float speed = 1.0f;


	void Awake()
	{
		this.cachedMeshFilter = this.gameObject.GetComponent<MeshFilter>();
		this.meshColors = new Color[this.cachedMeshFilter.sharedMesh.vertexCount];

		// Cache vertex indices for color change
		for (var i = 0; i < this.meshColors.Length; i++) {
			foreach (VertexColorItem item in this.colorItems) {
				if (this.cachedMeshFilter.mesh.colors[i] == item.originalColor) {
					item.targetVertices.Add(i);
					item.currentColor = item.originalColor;
					item.targetColor = item.originalColor;
					item.meshColors = this.cachedMeshFilter.mesh.colors;
				}
			}
		}
	}


	public void LateUpdate()
	{
		foreach (VertexColorItem item in this.colorItems) {
			if (item.targetColor != item.currentColor) {
				if (this.speed == 0.0) {
					this.cachedMeshFilter.mesh.colors = item.SetColor(item.targetColor);
				} else {
					this.cachedMeshFilter.mesh.colors = item.SetColor(Color.Lerp(item.currentColor, item.targetColor, Time.deltaTime * this.speed));
				}
			}
		}
	}


	// Set item color over time
	public void ChangeColor(string itemName, Color color, float speed = 1.0f)
	{
		this.speed = speed;

		foreach (VertexColorItem item in this.colorItems) {
			if (item.name == itemName) {
				item.targetColor = color;
			}
		}
	}


	// Reset item color to original value
	public void ResetItemColor(string itemName)
	{
		foreach (VertexColorItem item in this.colorItems) {
			if (item.name == itemName) {
				this.cachedMeshFilter.mesh.colors = item.SetColor(item.originalColor);
			}
		}
	}


	// Set item color instantly
	public void SetItemColor(string itemName, Color newColor)
	{
		foreach (VertexColorItem item in this.colorItems) {
			if (item.name == itemName) {
				item.targetColor = newColor;
				this.cachedMeshFilter.mesh.colors = item.SetColor(newColor);
			}
		}
	}

}


[System.Serializable]
public class VertexColorItem
{
	public string name;
	public Color originalColor;

	//[HideInInspector]
	public Color targetColor;

	//[HideInInspector]
	public Color currentColor;

	//[HideInInspector]
	public List<int> targetVertices = new List<int>();

	//[HideInInspector]
	public Color[] meshColors;


	public Color[] SetColor(Color newColor)
	{
		this.currentColor = newColor;

		for (var i = 0; i < this.targetVertices.Count; i++) {
			this.meshColors[this.targetVertices[i]] = newColor;
		}

		return this.meshColors;
	}
}
