using UnityEngine;
using System.Collections;

public class PulseTextureAlpha : MonoBehaviour
{

	private float duration = 1.0f;
	private Renderer cachedRenderer;

	public void Awake()
	{
		this.cachedRenderer = this.GetComponent<Renderer>();
	}

	public void FixedUpdate()
	{
		Color textureColor = this.cachedRenderer.material.color;
		textureColor.a = Mathf.PingPong(Time.time, duration) / duration;
		this.cachedRenderer.material.color = textureColor;
	}
}
