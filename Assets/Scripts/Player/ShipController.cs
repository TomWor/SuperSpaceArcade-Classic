using UnityEngine;
using System.Collections;
using PathologicalGames;

public class ShipController : MonoBehaviour
{
    public float startSpeed = 0.0f;
    public float defaultSpeed = 10.0f;
    public float currentSpeed = 0.0f;
    public float targetSpeed = 10.0f;
    public float acceleration = 10.0f;
    public float jumpSpeed = 20.0f;
    public AudioClip jumpSound;

    public float gravity = 10.0F;
    public bool superGround = false;
    public float superGroundGravity = 10.0F;
    public float horizontalSpeed = 10.0f;
    public float horizontalSpeedMobileMultiplier = 3.0f;

    public Transform gunShotPrefab1;
    public Transform gunShotPrefab2;
    public Transform gunShotPrefab3;

    private CharacterController controller;
    private Player player;
    protected Transform shipMesh;
    protected Transform shieldMesh;
    private Vector3 moveDirection = Vector3.zero;
    private Quaternion originalRotation;

    public float smooth = 0.0F;

    private float shipRotation;
    public Quaternion rotation = Quaternion.identity;


    void Awake()
    {
        this.controller = GetComponent<CharacterController>();
        this.originalRotation = transform.rotation;
        this.player = GetComponent<Player>();
        this.shipMesh = transform.Find("Mesh");
        this.shieldMesh = transform.Find("Shield");
        this.currentSpeed = this.startSpeed;
        this.targetSpeed = this.defaultSpeed;
    }


	public void OnEnable()
	{
        UIController.onShoot += this.Shoot;
        UIController.onJump += this.Jump;
	}


	public void OnDisable()
	{
        UIController.onShoot -= this.Shoot;
        UIController.onJump -= this.Jump;
	}


    void Start()
    {
        Camera mainCamera = Camera.main;         //GameObject.FindWithTag("MainCamera") as GameObject;
        mainCamera.gameObject.GetComponent<SmoothFollow>().target = GameObject.FindWithTag("PlayerViewTarget").GetComponent<Transform>();

        StartCoroutine(PauseCoroutine());
        StartCoroutine(AdjustSpeed());
    }


    void Update()
    {

        if (!GameController.isPaused())
        {

            if (this.controller.isGrounded)
            {
				this.moveDirection = new Vector3(0, 0, 0);
            }

            this.moveDirection.z = this.currentSpeed;

            if (this.superGround == true)
            {
                this.moveDirection.y -= this.superGroundGravity * this.gravity * Time.deltaTime; //+ (Input.GetAxis("Vertical") * 2.5f);
            }
            else
            {
                this.moveDirection.y -= this.gravity * Time.deltaTime; //+ (Input.GetAxis("Vertical") * 2.5f);
            }

			// Get horizontal axis steering
#if UNITY_EDITOR
		this.moveDirection.x = Input.GetAxis("Horizontal") * this.horizontalSpeed;
#elif UNITY_ANDROID
        this.moveDirection.x = Input.acceleration.x * this.horizontalSpeed * this.horizontalSpeedMobileMultiplier;
#elif UNITY_IOS
        this.moveDirection.x = Input.acceleration.x * this.horizontalSpeed * this.horizontalSpeedMobileMultiplier;
#endif

            this.moveDirection = transform.TransformDirection(moveDirection);
            this.controller.Move(moveDirection * Time.deltaTime);


            if (Input.GetButtonDown("Fire3"))
            {
                Debug.Break();
            }

            if ((Input.GetAxis("Horizontal") > 0.3f || Input.GetAxis("Horizontal") < -0.3f) || (Input.acceleration.x > 0.1f || Input.acceleration.x < -0.1f))
            {

                if (shipMesh.rotation.z > -0.3 && shipMesh.rotation.z < 0.3)
                {

#if UNITY_EDITOR
				this.shipRotation = Input.GetAxis("Horizontal") * 50 * Time.deltaTime * -1 * 2.0f;
#elif UNITY_ANDROID
				this.shipRotation = Input.acceleration.x * 50 * Time.deltaTime * -1 * 2.0f * this.horizontalSpeedMobileMultiplier;
#elif UNITY_IOS
				this.shipRotation = Input.acceleration.x * 50 * Time.deltaTime * -1 * 2.0f * this.horizontalSpeedMobileMultiplier;
#endif

                    this.shipMesh.Rotate(Vector3.forward * this.shipRotation);
                    this.shieldMesh.Rotate(Vector3.forward * this.shipRotation);
                }

            }
            else
            {
                this.shipMesh.rotation = this.shieldMesh.rotation = Quaternion.Lerp(this.shipMesh.rotation, this.originalRotation, Time.deltaTime * 4.0f);
            }

        }
    }


    public void Shoot()
    {
        if (player.weaponStatus == 2)
        {
            PoolManager.Pools["Shots"].Spawn(this.gunShotPrefab2, new Vector3(transform.position.x, transform.position.y, transform.position.z), this.shipMesh.rotation);
        }
        else if (player.weaponStatus == 3)
        {
            PoolManager.Pools["Shots"].Spawn(this.gunShotPrefab3, new Vector3(transform.position.x, transform.position.y, transform.position.z), this.shipMesh.rotation);
        }
        else
        {
            PoolManager.Pools["Shots"].Spawn(this.gunShotPrefab1, new Vector3(transform.position.x, transform.position.y, transform.position.z), this.shipMesh.rotation);
        }
    }


    public void Jump()
    {
		// Set superground when jump action is triggered mid-air
		this.superGround = !this.controller.isGrounded;

        this.GetComponent<AudioSource>().PlayOneShot(this.jumpSound, 1.5f);
        this.moveDirection.y = this.jumpSpeed;

        this.moveDirection = transform.TransformDirection(this.moveDirection);
        this.controller.Move(this.moveDirection * Time.deltaTime);
    }

    /*
       void OnGUI () {

       if (GUI.Button(new Rect(20, 480, 200, 50), "Jump")) {
       audio.PlayOneShot(jumpSound);
       moveDirection.y = jumpSpeed;
       }
       if (GUI.Button(new Rect(800, 480, 200, 50), "Shoot")) {
       if (player.weaponStatus == 2) {
       Instantiate(gunShotPrefab, new Vector3(transform.position.x-1, transform.position.y, transform.position.z), Quaternion.identity);
       Instantiate(gunShotPrefab, new Vector3(transform.position.x+1, transform.position.y, transform.position.z), Quaternion.identity);
       audio.PlayOneShot(gunShotSound2);
       }
       else if (player.weaponStatus == 3) {
       Instantiate(gunShotPrefab, new Vector3(transform.position.x-1.5f, transform.position.y, transform.position.z), Quaternion.identity);
       Instantiate(gunShotPrefab, new Vector3(transform.position.x+1.5f, transform.position.y, transform.position.z), Quaternion.identity);
       Instantiate(gunShotPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
       audio.PlayOneShot(gunShotSound3);
       } else {
       Instantiate(gunShotPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
       audio.PlayOneShot(gunShotSound1);
       }
       }

       }
     */


    IEnumerator AdjustSpeed()
    {
        while (true)
        {
            if (this.currentSpeed != this.targetSpeed)
            {

                if (this.currentSpeed - this.targetSpeed <= 0.5f && this.currentSpeed - this.targetSpeed >= -0.5f)
                {

                    //Debug.Log(this.currentSpeed + " ===> " + this.targetSpeed);
                    this.currentSpeed = this.targetSpeed;

                }
                else
                {

                    this.currentSpeed = Mathf.Lerp(this.currentSpeed, this.targetSpeed, this.acceleration * Time.deltaTime);
                    //Debug.Log(this.currentSpeed + " -> " + this.targetSpeed);

                }
            }
            yield return new WaitForFixedUpdate();
        }
    }


    // Pause Coroutine
    IEnumerator PauseCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                else
                {
                    Time.timeScale = 0;
                }
            }
            yield return null;
        }
    }

}
