using UnityEngine;
using System.Collections;
using PathologicalGames;

public class Enemy : Destructible
{
	protected MeshRenderer cachedMeshRenderer;

	public Color hitColor;
	public Color blinkColor;
	public Color originalColor;



	// vertex color components of the mesh items
	private ChangeVertexColor[] vertexColorComponents;


	public void Awake()
	{
		this.cachedMeshRenderer = this.GetComponentInChildren<MeshRenderer>();
		this.cachedTransform = this.transform;

		this.originalPosition = this.cachedTransform.localPosition;
		this.ResetDefaultValues();

		this.vertexColorComponents = this.GetComponentsInChildren<ChangeVertexColor>();
	}


	public void OnSpawned()
	{
		if (!this.isSpawned) {

			this.pool = PoolManager.Pools["Spawns"];
			EventManager.onPlayerInvulnerable += this.OnPlayerInvulnerable;

			this.isSpawned = true;

			foreach (ChangeVertexColor vertexColorComponent in this.vertexColorComponents) {
				vertexColorComponent.ChangeColor("EnemyColor", this.originalColor, 0.0f);
			}
		}
	}


	public void OnDespawned()
	{
		//Debug.Log( "OnDespawned: " + this.gameObject.ToString() );
		this.ResetDefaultValues();
		EventManager.onPlayerInvulnerable -= this.OnPlayerInvulnerable;
		StopAllCoroutines();
	}


	public void OnPlayerInvulnerable(bool invulnerable)
	{
		if (invulnerable && this.gameObject.activeSelf) {
			StartCoroutine(Blink());
		} else {
			StopCoroutine(Blink());
		}
	}


	protected IEnumerator Blink()
	{
		while (true) {

			foreach (ChangeVertexColor vertexColorComponent in this.vertexColorComponents) {
				vertexColorComponent.ChangeColor("EnemyColor", this.blinkColor, 0.0f);
			}

			yield return new WaitForSeconds(0.5f);

			foreach (ChangeVertexColor vertexColorComponent in this.vertexColorComponents) {
				vertexColorComponent.ChangeColor("EnemyColor", this.originalColor, 0.0f);
			}

			yield return new WaitForSeconds(0.5f);
		}
	}


	public new void ApplyDamage(int damage)
	{
		//Debug.Log( "Apply Damage. Current health: " + this._health + " exploded: " + this.exploded.ToString() );
		this._health -= damage;
		if (this._health <= 0) {
			this.Explode();
		} else {
			foreach (ChangeVertexColor vertexColorComponent in this.vertexColorComponents) {
				vertexColorComponent.ChangeColor("EnemyColor", this.hitColor, 0.0f);
			}
		}
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


	public void OnTriggerEnter(Collider other)
	{
		if (!TrackGenerator.trackResetActive) {
			if (other.gameObject.tag == "Player") {
				this.Explode();
				other.SendMessage("Collision", 100, SendMessageOptions.DontRequireReceiver);
			}
		}

	}

}
