using UnityEngine;
using System.Collections;

namespace SuperSpaceArcade
{
	public class Enemy_Barrier : Enemy
	{

		public int materialIndex = 0;
		public Vector2 uvAnimationRate = new Vector2(1.0f, 1.0f);
		public string textureName = "_MainTex";

		Vector2 uvOffset = Vector2.zero;

		void LateUpdate()
		{
			uvOffset += (uvAnimationRate * Time.deltaTime);
			if (this.cachedMeshRenderer.enabled) {
				this.cachedMeshRenderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
			}
		}

	}
}