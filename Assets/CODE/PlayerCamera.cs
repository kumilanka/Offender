using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour 
{
    public Transform followTarget;

    private Vector3 currentPos;
    public float smoothTime = 1f;
    public float maxSpeed = 1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 targetPos = new Vector3(followTarget.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentPos, smoothTime, maxSpeed, Time.deltaTime);
	}
}
