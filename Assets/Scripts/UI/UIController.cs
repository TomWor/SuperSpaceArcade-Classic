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


	public void Shoot ()
	{
		if ( onShoot != null )
			onShoot ();
	}


	public void Jump ()
	{
		if ( onJump != null )
			onJump ();
	}

}
