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
        if (followTarget != null) // if the followtarget is there, follow it using smoothdamp
        {
            Vector3 targetPos = new Vector3(followTarget.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentPos, smoothTime, maxSpeed, Time.deltaTime);
        }
        else // otherwise look for player and jump the camera to the player when found, then start following again
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                followTarget = player.GetComponent<Player>().cameraFollowPosition;
                WarpCamera();
            }
        }
	}

    private void WarpCamera()
    {
        Vector3 targetPos = new Vector3(followTarget.position.x, transform.position.y, transform.position.z);
        transform.position = targetPos;
    }
}
