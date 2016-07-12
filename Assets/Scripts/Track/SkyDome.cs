using UnityEngine;
using System.Collections;
using PathologicalGames;

namespace SuperSpaceArcade
{
	public class SkyDome : MonoBehaviour
	{
		private Color originalSkyColor = Color.white;
		private Color targetSkyColor;
		private Color currentSkyColor;

		private Color targetAmbientColor;
		private Color currentAmbientColor;

		public Color skyColor;
		private MeshRenderer cachedMeshRenderer;

		private Light sun;


		public void Awake()
		{
			this.cachedMeshRenderer = this.GetComponent<MeshRenderer>();
			this.currentSkyColor = this.originalSkyColor;
			this.targetSkyColor = this.originalSkyColor;
			this.currentAmbientColor = new Color(0.9f, 0.95f, 1.0f);
			this.sun = GameObject.FindWithTag("Sun").GetComponent<Light>();

			this.cachedMeshRenderer.sharedMaterial.color = this.currentSkyColor;
			RenderSettings.ambientLight = this.currentAmbientColor;
		}


		public void OnEnable()
		{
			//EventManager.onTrackBorderColorChanged += this.OnTrackBorderColorChanged;
			EventManager.onPlayerSpawned += this.OnPlayerSpawned;
			//EventManager.onPlayerDestroyed += this.OnPlayerDestroyed;
			EventManager.onMenuEnter += this.OnMenuEnter;
		}


		public void OnDisable()
		{
			//EventManager.onTrackBorderColorChanged -= this.OnTrackBorderColorChanged;
			EventManager.onPlayerSpawned -= this.OnPlayerSpawned;
			//EventManager.onPlayerDestroyed -= this.OnPlayerDestroyed;
			EventManager.onMenuEnter -= this.OnMenuEnter;
		}


		public void OnPlayerSpawned(TrackSpectator player)
		{
			this.GetComponent<TransformConstraint>().target = player.transform;
		}


		public void OnMenuEnter()
		{
			this.GetComponent<TransformConstraint>().target = Camera.main.transform;
		}


		public void FixedUpdate()
		{

			if (this.currentSkyColor != this.targetSkyColor) {

				this.currentSkyColor = Color.Lerp(this.currentSkyColor, this.targetSkyColor, Time.deltaTime);
				this.cachedMeshRenderer.sharedMaterial.color = this.currentSkyColor;

				this.currentAmbientColor = Color.Lerp(this.currentAmbientColor, this.targetAmbientColor, Time.deltaTime);
				RenderSettings.ambientLight = this.currentAmbientColor;
			}

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
}