using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class Score : MonoBehaviour
{
	private Transform cachedTransform;
	private string poolName = "Score";
	private List<Transform> characterTransforms = new List<Transform>();

	public float lifespan = 2.0f;
	public int speed = 30;

	private int _score;
	public int ScoreValue
	{
		get { return this._score; }
		set
		{
			this._score = value;
			this.CreateScoreString();
		}

	}


	public void Awake()
	{
		this.cachedTransform = this.transform;
	}


	public void OnSpawned()
	{
		StartCoroutine( Rise() );
		PoolManager.Pools[this.poolName].Despawn( this.transform, this.lifespan, PoolManager.Pools[this.poolName].transform );
	}


	private void CreateScoreString()
	{
		string scoreString = this._score.ToString();
		float characterPositionX = -( ( scoreString.Length * 15 ) / 2 );

		foreach ( char c in scoreString )
		{
			Vector3 characterPosition = new Vector3( this.cachedTransform.position.x + characterPositionX, this.cachedTransform.position.y, this.cachedTransform.position.z );

			Transform characterTransform = PoolManager.Pools["Alphabet"].Spawn( c.ToString(), characterPosition, Quaternion.identity, this.cachedTransform );
			this.characterTransforms.Add( characterTransform );

			characterPositionX += 15;
		}

	}


	public void OnDespawned()
	{
		foreach ( Transform character in this.characterTransforms )
		{
			PoolManager.Pools["Alphabet"].Despawn( character, PoolManager.Pools["Alphabet"].transform );
		}
		this.characterTransforms.Clear();
		StopAllCoroutines();
	}


	private IEnumerator Rise()
	{
		while ( true )
		{
			this.cachedTransform.position = new Vector3( this.cachedTransform.position.x, this.cachedTransform.position.y + this.speed * Time.deltaTime, this.cachedTransform.position.z );
			yield return new WaitForFixedUpdate();
		}
	}

}
