using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour
{

	public delegate void OnShootHandler ();

	public static event OnShootHandler onShoot;

	public delegate void OnJumpHandler ();

	public static event OnJumpHandler onJump;


	public void Awake ()
	{
		//UIController.onShoot += delegate { Debug.Log ( "onShoot delegate" ); };
	}


	public void Update ()
	{
#if UNITY_EDITOR
		if (Input.GetButtonDown ("Fire1") || Input.GetKeyDown ("s")) {
			this.Shoot ();
		}

		if (Input.GetButtonDown ("Jump") || Input.GetButtonDown ("Fire2")) {
			this.Jump ();
		}
#endif
	}


	public void Shoot ()
	{
		if (onShoot != null)
			onShoot ();
	}


	public void Jump ()
	{
		if (onJump != null)
			onJump ();
	}

}
