using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine.UI;


public class Player : TrackRider
{
    private Transform cachedTransform;

    public int points = 0;
    public int health = 50;
    public bool invulnerable = false;

    public int multiplicator = 3;
    public int weaponStatus = 1;
    public int stressLevel = 3;

    public GameObject weaponUpgrade1;
    public GameObject weaponUpgrade2;

    public AudioClip hitSound;
    private Transform shipMesh;
    public GameObject explodePrefab;

    public bool gameOver = false;
    private GameObject gameOverUI;
    private GameObject inGameUI;


    public void OnEnable()
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
        EventManager.onPlayerAddPoints += this.OnPlayerAddPoints;

        StartCoroutine(CheckForFallingDeath(this.trackGenerator));
    }


    protected new IEnumerator CheckForFallingDeath(TrackGenerator trackGenerator)
    {
        while (!this.gameOver)
        {
            if (this.cachedTransform.position.y < trackGenerator.CurrentTrackTileVerticalOffset - 100)
            {
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
        if (!TrackGenerator.trackResetActive && !this.invulnerable && !this.gameOver)
        {
            this.health -= hitPoints;
            this.UpdateStatus();
            this.weaponStatus = 1;

            if (this.health <= 0)
            {
                this.GameOver();
            }
        }
    }


    public void HealthPowerUp(int healthPoints)
    {
        if (health >= 100)
        {
            this.PowerUpPoints(healthPoints);
            this.health = 100;
        }
        else
        {
            this.health += healthPoints;
        }

        this.UpdateStatus();
    }


    public void PowerUpPoints(int addPoints)
    {
        this.points += addPoints * this.multiplicator;
    }


    void UpdateStatus()
    {
    }


    public void UpgradeWeapon()
    {
        if (weaponStatus < 3)
        {
            weaponStatus++;

            switch (weaponStatus)
            {

                case 2:
                    this.weaponUpgrade1.SetActive(true);
                    break;

                case 3:
                    this.weaponUpgrade1.SetActive(false);
                    this.weaponUpgrade2.SetActive(true);
                    break;

            }
        }
        else
        {
            weaponStatus = 3;
            this.PowerUpPoints(256);
        }
    }


    public void GameOver()
    {
        Destroy(this.gameObject.GetComponent<ShipController>());
        Destroy(shipMesh.gameObject);

        gameOver = true;
        this.gameOverUI.transform.Find("ScorePanel/Score").GetComponent<Text>().text = this.points.ToString();
        this.gameOverUI.SetActive(true);
        this.inGameUI.SetActive(false);

        GameObject shipExploded = Instantiate(explodePrefab, transform.position, Quaternion.identity) as GameObject;
        Rigidbody[] rigidbodies = shipExploded.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody body in rigidbodies)
        {
            body.AddExplosionForce(Random.Range(4000, 12000), new Vector3(transform.position.x, transform.position.y, transform.position.z - 10.0f), Random.Range(2, 10), 3.0f);
        }

    }

}
