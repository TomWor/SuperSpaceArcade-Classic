using UnityEngine;
using System.Collections;

public class MineDetectionRadius : MonoBehaviour {

	[HideInInspector]
	public Transform parent;


	public void Awake ()
	{
		this.parent = this.transform.parent.parent;
	}


	public void OnTriggerEnter ( Collider other)
	{
		if ( other.gameObject.tag == "Player" )
		{
			this.parent.GetComponent<Enemy>().Explode();
			//Debug.Log("Mine detected player");
		}
	}

}
