using UnityEngine;
using System.Collections;
using PathologicalGames;

public class SideSpawn : MonoBehaviour
{
	// Length of the SideSpawn anchor
	public int anchorLength = 50;

	// Offsets for how far the SideSpawn reaches out forwards
	// or backwards
	public int forwardOffset = 50;
	public int backwardOffset = 0;

	public int likelyhood = 5;

	// Left or right SideSpawn? 0 = left, 1 = right
	public int spawnType = 0;

	// Cached Transform
	private Transform cachedTransform;

	[HideInInspector]
	public string poolName = "Track";


	public void Awake()
	{
		this.cachedTransform = this.transform;
	}


	public void Despawn()
	{
		PoolManager.Pools[this.poolName].Despawn( this.cachedTransform );
	}
}
