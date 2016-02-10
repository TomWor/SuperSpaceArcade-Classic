using UnityEngine;
using System.Collections;
using PathologicalGames;

public class Container : Destructible
{
	public void Awake()
	{
		this.cachedTransform = this.transform;
		this.originalPosition = this.cachedTransform.localPosition;
		this.ResetDefaultValues();
	}


	public void OnSpawned()
	{
		if (!this.isSpawned) {
			this.pool = PoolManager.Pools["Spawns"];
			this.isSpawned = true;
		}
	}


	public void OnDespawned()
	{
		this.ResetDefaultValues();
	}


	public void ResetDefaultValues()
	{
		this.transform.localPosition = this.originalPosition;
		this._health = this.health;
		this.exploded = false;
		this.isSpawned = false;

		// Get all children with the SaveRestoreTransform component
		SaveRestoreTransform[] children = this.transform.GetComponentsInChildren<SaveRestoreTransform>();

		// Call restore method on every child
		foreach (SaveRestoreTransform child in children) {
			child.RestoreTransform();
		}
	}
}
