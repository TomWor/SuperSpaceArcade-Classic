using UnityEngine;
using System.Collections;
using PathologicalGames;

namespace SuperSpaceArcade
{
	public class PowerUpEffect : MonoBehaviour
	{
		public GameObject effectMesh;
		public GameObject powerUpMesh;

		private Transform cachedTransform;

		private Transform effectMeshTransform;
		private MeshRenderer effectMeshRenderer;
		private Color effectMeshOriginalColor;

		private Transform powerUpMeshTransform;
		private MeshRenderer powerUpMeshRenderer;
		private Color powerUpMeshOriginalColor;

		private string poolName = "Effects";

		public string PoolName {
			get { return this.poolName; }
		}


		public void Awake()
		{
			this.cachedTransform = this.transform;

			this.effectMeshTransform = this.effectMesh.transform;
			this.effectMeshRenderer = this.effectMesh.GetComponent<MeshRenderer>();
			this.effectMeshOriginalColor = this.effectMeshRenderer.material.GetColor("_Color");

			this.powerUpMeshTransform = this.powerUpMesh.transform;
			this.powerUpMeshRenderer = this.powerUpMesh.GetComponent<MeshRenderer>();
			this.powerUpMeshOriginalColor = this.powerUpMeshRenderer.material.GetColor("_Color");
		}


		public void OnSpawned()
		{
			PoolManager.Pools[this.poolName].Despawn(this.cachedTransform, 2.0f, PoolManager.Pools[this.poolName].transform);
			StartCoroutine(Rise());
		}


		public void OnDespawned()
		{
			StopAllCoroutines();

			this.effectMeshRenderer.material.SetColor("_Color", this.effectMeshOriginalColor);
			this.effectMeshTransform.localPosition = Vector3.zero;

			if (this.powerUpMeshTransform is Transform) {
				this.powerUpMeshRenderer.material.SetColor("_Color", this.powerUpMeshOriginalColor);
				this.powerUpMeshTransform.localPosition = Vector3.zero;
			}
		}


		private IEnumerator Rise()
		{
			while (true) {
				this.effectMeshTransform.position += Vector3.up;
				this.effectMeshTransform.Rotate(new Vector3(0, 0, -6.0f));

				Color effectColor = this.effectMeshRenderer.material.GetColor("_Color");
				effectColor.a -= 0.03f;
				this.effectMeshRenderer.material.SetColor("_Color", effectColor);

				if (this.powerUpMeshTransform is Transform) {
					this.powerUpMeshTransform.position += Vector3.up * 1.2f;
					this.powerUpMeshTransform.Rotate(new Vector3(0, 4.8f, 0));

					Color powerUpColor = this.powerUpMeshRenderer.material.GetColor("_Color");
					powerUpColor.a -= 0.03f;
					this.powerUpMeshRenderer.material.SetColor("_Color", powerUpColor);
				}

				yield return new WaitForFixedUpdate();
			}
		}

	}
}