using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine.UI;
using DarkTonic.MasterAudio;


public class Player : TrackRider
{
	private Transform cachedTransform;

	public int points = 0;

	public bool invulnerable = false;
	// Invulnerability left (in seconds)
	private int invulnerabilityCountdown = 0;

	public int multiplicator = 3;
	public int weaponStatus = 1;
	public int stressLevel = 3;

	public GameObject weaponUpgrade1;
	public GameObject weaponUpgrade2;
	public int weaponUpgradeCountdown = 10;

	private Transform shipMesh;
	private Transform shieldMesh;
	public GameObject explodePrefab;

	public bool gameOver = false;
	private GameObject gameOverUI;
	private GameObject inGameUI;


	public new void OnEnable()
	{
		this.gameOverUI = GameObject.FindWithTag("GameOverUI");
		this.gameOverUI.SetActive(false);
		this.inGameUI = GameObject.FindWithTag("InGameUI");
	}


	public void OnDisable()
	{
		StopAllCoroutines();
	}


	public void Awake()
	{
		this.cachedTransform = this.transform;
	}


	public override void OnTrackCreated(TrackGenerator trackGenerator)
	{
		base.OnTrackCreated(trackGenerator);
		StartCoroutine(this.CheckForFallingDeath(trackGenerator));
	}


	public void Start()
	{
		shipMesh = this.cachedTransform.Find("Mesh");
		shieldMesh = this.cachedTransform.Find("Shield");
		EventManager.onPlayerAddPoints += this.OnPlayerAddPoints;

		//StartCoroutine(CheckForFallingDeath(this.trackGenerator));
		StartCoroutine(this.WeaponUpgradeCountdown());
	}


	protected IEnumerator CheckForFallingDeath(TrackGenerator trackGenerator)
	{
		while (!this.gameOver) {
			if (this.cachedTransform.position.y < trackGenerator.CurrentTrackTileVerticalOffset - 100) {
				this.GameOver();
			}
			yield return new WaitForSeconds(0.5f);
		}
	}


	public void OnPlayerAddPoints(int value, Vector3 sourcePosition, Quaternion sourceRotation)
	{
		this.points += value;
	}


	public void Collision(int hitPoints)
	{
		if (!TrackGenerator.trackResetActive && !this.invulnerable && !this.gameOver) {
			this.GameOver();
		}
	}


	public void PowerUpPoints(int addPoints)
	{
		this.points += addPoints * this.multiplicator;
	}


	public void UpgradeWeapon()
	{
		if (this.weaponStatus < 3) {

			this.weaponStatus++;
			Debug.Log("WeaponUPGRADE: " + this.weaponStatus);

			switch (this.weaponStatus) {

			case 2:
				this.weaponUpgrade1.SetActive(true);
				break;

			case 3:
				this.weaponUpgrade2.SetActive(true);
				break;

			}

		} else {
			this.PowerUpPoints(256);
		}

		this.weaponUpgradeCountdown += 10;
	}


	public IEnumerator WeaponUpgradeCountdown()
	{
		while (!this.gameOver) {

			//Debug.Log(this.weaponUpgradeCountdown);

			if (this.weaponStatus >= 2) {

				this.weaponUpgradeCountdown--;

				if (this.weaponUpgradeCountdown <= 5) {

					if (this.weaponStatus == 3) {
						this.weaponUpgrade2.GetComponent<PulseTextureAlpha>().enabled = true;
					} else if (this.weaponStatus == 2) {
						this.weaponUpgrade1.GetComponent<PulseTextureAlpha>().enabled = true;
					} 
				}

				if (this.weaponUpgradeCountdown <= 0) {

					this.weaponStatus--;
					//Debug.Log("WeaponStatus: " + this.weaponStatus);

					if (this.weaponStatus == 2) {
						this.weaponUpgrade2.SetActive(false);
					} else if (this.weaponStatus <= 1) {
						this.weaponUpgrade1.SetActive(false);
					} 

					this.weaponUpgrade1.GetComponent<PulseTextureAlpha>().enabled = false;
					this.weaponUpgrade2.GetComponent<PulseTextureAlpha>().enabled = false;

					this.weaponUpgradeCountdown += 10;
				}
			}

			yield return new WaitForSeconds(1.0f);
		}
	}


	public void Invulnerability()
	{
		GameController.PlayerInvulnerable = true;
		this.invulnerabilityCountdown = 10;
		MasterAudio.FireCustomEvent("Invulnerability", this.cachedTransform.position);
		this.shieldMesh.gameObject.SetActive(true);
		StartCoroutine(this.InvulnerabilityCountdown());
	}


	protected IEnumerator InvulnerabilityCountdown()
	{
		while (true) {

			//Debug.Log("Invulnerable for " + this.invulnerabilityCountdown + " seconds.");

			if (this.invulnerabilityCountdown > 0) {
				this.invulnerable = true;
				this.invulnerabilityCountdown--;
			} else {
				this.invulnerable = false;
				MasterAudio.FireCustomEvent("InvulnerabilityOver", this.cachedTransform.position);
				GameController.PlayerInvulnerable = false;
				this.shieldMesh.gameObject.SetActive(false);
				yield break;
			}

			yield return new WaitForSeconds(1.0f);
		}
	}

	public void GameOver()
	{
		StopAllCoroutines();
		Destroy(this.gameObject.GetComponent<ShipController>());
		Destroy(shipMesh.gameObject);

		gameOver = true;
		this.gameOverUI.transform.Find("ScorePanel/Score").GetComponent<Text>().text = this.points.ToString();
		this.gameOverUI.SetActive(true);
		this.inGameUI.SetActive(false);

		GameObject shipExploded = Instantiate(explodePrefab, transform.position, Quaternion.identity) as GameObject;
		Rigidbody[] rigidbodies = shipExploded.GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody body in rigidbodies) {
			body.AddExplosionForce(Random.Range(4000, 12000), new Vector3(transform.position.x, transform.position.y, transform.position.z - 10.0f), Random.Range(2, 10), 3.0f);
		}

	}

}
