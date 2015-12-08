using UnityEngine;
using System.Collections;

public class RingRotation : MonoBehaviour
{

	// Update is called once per frame
	void FixedUpdate()
	{

		this.transform.Rotate( Vector3.forward, 1.0f );

	}
}
