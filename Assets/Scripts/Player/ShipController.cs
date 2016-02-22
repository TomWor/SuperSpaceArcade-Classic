using UnityEngine;
using System.Collections;
using PathologicalGames;
using DarkTonic.MasterAudio;

namespace SuperSpaceArcade
{
	public class ShipController : MonoBehaviour
	{
		private Transform cachedTransform;

		public float startSpeed = 0.0f;
		public float defaultSpeed = 10.0f;
		public float currentSpeed = 0.0f;
		public float targetSpeed = 10.0f;
		public float acceleration = 10.0f;
		public float jumpSpeed = 20.0f;

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
			this.cachedTransform = this.transform;
			this.controller = GetComponent<CharacterController>();
			this.originalRotation = this.cachedTransform.rotation;
			this.player = GetComponent<Player>();
			this.shipMesh = this.cachedTransform.Find("Mesh");
			this.shieldMesh = this.cachedTransform.Find("Shield");
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
			StartCoroutine(PauseCoroutine());
			StartCoroutine(AdjustSpeed());
		}


		void Update()
		{

			if (!GameController.isPaused()) {

				if (this.controller.isGrounded) {
					this.moveDirection = new Vector3(0, 0, 0);
					this.superGround = false;
				}

				this.moveDirection.z = this.currentSpeed;

				if (this.superGround == true) {
					this.moveDirection.y -= this.superGroundGravity * this.gravity * Time.deltaTime; //+ (Input.GetAxis("Vertical") * 2.5f);
				} else {
					this.moveDirection.y -= this.gravity * Time.deltaTime; //+ (Input.GetAxis("Vertical") * 2.5f);
				}

				// Get horizontal axis steering
#if UNITY_EDITOR
				this.moveDirection.x = Input.GetAxis("Horizontal") * this.horizontalSpeed;
#elif UNITY_STANDALONE
			this.moveDirection.x = Input.GetAxis("Horizontal") * this.horizontalSpeed;
#elif UNITY_ANDROID
        this.moveDirection.x = Input.acceleration.x * this.horizontalSpeed * this.horizontalSpeedMobileMultiplier;
#elif UNITY_IOS
        this.moveDirection.x = Input.acceleration.x * this.horizontalSpeed * this.horizontalSpeedMobileMultiplier;
#endif

				this.moveDirection = transform.TransformDirection(moveDirection);
				this.controller.Move(moveDirection * Time.deltaTime);


				if (Input.GetButtonDown("Fire3")) {
					Debug.Break();
				}

#if UNITY_EDITOR

#elif UNITY_STANDALONE
			if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.S)) {
				this.Shoot();
			}

			if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) {
				this.Jump();
			}
#endif
				if ((Input.GetAxis("Horizontal") > 0.3f || Input.GetAxis("Horizontal") < -0.3f) || (Input.acceleration.x > 0.1f || Input.acceleration.x < -0.1f)) {

					if (shipMesh.rotation.z > -0.3 && shipMesh.rotation.z < 0.3) {

#if UNITY_EDITOR
						this.shipRotation = Input.GetAxis("Horizontal") * 50 * Time.deltaTime * -1 * 2.0f;
#elif UNITY_STANDALONE
					this.shipRotation = Input.GetAxis("Horizontal") * 50 * Time.deltaTime * -1 * 2.0f;
#elif UNITY_ANDROID
				this.shipRotation = Input.acceleration.x * 50 * Time.deltaTime * -1 * 2.0f * this.horizontalSpeedMobileMultiplier;
#elif UNITY_IOS
				this.shipRotation = Input.acceleration.x * 50 * Time.deltaTime * -1 * 2.0f * this.horizontalSpeedMobileMultiplier;
#endif

						this.shipMesh.Rotate(Vector3.forward * this.shipRotation);
						this.shieldMesh.Rotate(Vector3.forward * this.shipRotation);
					}

				} else {
					this.shipMesh.rotation = this.shieldMesh.rotation = Quaternion.Lerp(this.shipMesh.rotation, this.originalRotation, Time.deltaTime * 4.0f);
				}

			}
		}


		public void Shoot()
		{
			if (player.weaponStatus == 2) {
				PoolManager.Pools["Shots"].Spawn(this.gunShotPrefab2, new Vector3(transform.position.x, transform.position.y, transform.position.z), this.shipMesh.rotation);
			} else if (player.weaponStatus == 3) {
				PoolManager.Pools["Shots"].Spawn(this.gunShotPrefab3, new Vector3(transform.position.x, transform.position.y, transform.position.z), this.shipMesh.rotation);
			} else {
				PoolManager.Pools["Shots"].Spawn(this.gunShotPrefab1, new Vector3(transform.position.x, transform.position.y, transform.position.z), this.shipMesh.rotation);
			}
		}


		public void Jump()
		{
			// Set superground when jump action is triggered mid-air
			this.superGround = !this.controller.isGrounded;

			if (this.controller.isGrounded) {
				MasterAudio.FireCustomEvent("Jump", this.cachedTransform.position);
				this.moveDirection.y = this.jumpSpeed;
			}

			this.moveDirection = transform.TransformDirection(this.moveDirection);
			this.controller.Move(this.moveDirection * Time.deltaTime);
		}


		IEnumerator AdjustSpeed()
		{
			while (true) {
				if (this.currentSpeed != this.targetSpeed) {

					if (this.currentSpeed - this.targetSpeed <= 0.5f && this.currentSpeed - this.targetSpeed >= -0.5f) {

						//Debug.Log(this.currentSpeed + " ===> " + this.targetSpeed);
						this.currentSpeed = this.targetSpeed;

					} else {

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
			while (true) {
				if (Input.GetKeyDown(KeyCode.Escape)) {
					if (Time.timeScale == 0) {
						Time.timeScale = 1;
					} else {
						Time.timeScale = 0;
					}
				}
				yield return null;
			}
		}

	}
}