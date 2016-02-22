using UnityEngine;
using System.Collections;

namespace SuperSpaceArcade
{
	public class Debris : MonoBehaviour
	{
		/// <summary>
		/// On DeSpawned event, fired by PoolManager, all the child elements to the debris prefab
		/// get restored to their original position, so they generate a proper new explosion when
		/// respawned from the pool
		/// </summary>
		public void OnDespawned()
		{
			// Get all children with the SaveRestoreTransform component
			SaveRestoreTransform[] children = this.transform.GetComponentsInChildren<SaveRestoreTransform>();

			// Call restore method on every child
			foreach (SaveRestoreTransform child in children) {
				child.RestoreTransform();
			}
		}

	}
}