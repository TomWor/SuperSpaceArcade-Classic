using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{

    private Vector3 startPosition;
    private Transform cachedTransform;
    private int amount = 200;
    private float speed = 70.0f;


    public void Awake()
    {

        this.startPosition = this.transform.position;
        this.cachedTransform = this.transform;

    }


    public void FixedUpdate()
    {

        this.cachedTransform.localPosition = new Vector3(this.startPosition.x + (Mathf.PingPong(Time.time * this.speed, this.amount) - this.amount / 2), this.startPosition.y, this.startPosition.z);

    }
}
