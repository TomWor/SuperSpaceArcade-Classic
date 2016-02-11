using UnityEngine;
using System.Collections;

public class PulseTextureAlpha : MonoBehaviour
{
	private Color originalColor;
	private float duration = 1.0f;
	private Renderer cachedRenderer;

	public void Awake()
	{
		this.cachedRenderer = this.GetComponent<Renderer>();
		this.originalColor = this.cachedRenderer.material.color;
	}

	public void FixedUpdate()
	{
		Color textureColor = this.cachedRenderer.material.color;
		textureColor.a = Mathf.PingPong(Time.time, duration) / duration;
		this.cachedRenderer.material.color = textureColor;
	}

	public void OnDisable()
	{
		this.cachedRenderer.material.color = this.originalColor;
	}
}
