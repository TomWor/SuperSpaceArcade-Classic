using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        for (var i = 0; i < this.meshColors.Length; i++)
        {
            foreach (VertexColorItem item in this.colorItems)
            {
                if (this.cachedMeshFilter.mesh.colors[i] == item.originalColor)
                {
                    item.targetVertices.Add(i);
                    item.currentColor = item.originalColor;
                }
            }
        }
    }


    void LateUpdate()
    {
        foreach (VertexColorItem item in this.colorItems)
        {
            if (item.targetColor != item.currentColor)
            {
                this.meshColors = this.cachedMeshFilter.mesh.colors;

				if (this.speed == 0.0)
				{
					item.currentColor = item.targetColor;
				} else {
					item.currentColor = Color.Lerp(item.currentColor, item.targetColor, Time.deltaTime * this.speed);
				}

                for (var i = 0; i < item.targetVertices.Count; i++)
                {
                    this.meshColors[item.targetVertices[i]] = item.currentColor;
                }

                this.cachedMeshFilter.mesh.colors = this.meshColors;
            }
        }
    }


    public void ChangeColor(string itemName, Color color, float speed = 1.0f)
    {
		this.speed = speed;

        foreach (VertexColorItem item in this.colorItems)
        {
            if (item.name == itemName)
            {
                item.targetColor = color;
            }

        }
    }

}


[System.Serializable]
public class VertexColorItem
{
    public string name;
    public Color originalColor;

    [HideInInspector]
    public Color targetColor;

    [HideInInspector]
    public Color currentColor;

    [HideInInspector]
    public List<int> targetVertices = new List<int>();
}
