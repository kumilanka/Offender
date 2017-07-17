using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAroundEdges : MonoBehaviour
{
    public bool moveCamera = false;
    public GameObject graphics;

	// Use this for initialization
	void Start ()
    {
        if (graphics != null)
        {
            GameObject leftMirage = Instantiate(graphics, graphics.transform.parent);
            leftMirage.transform.position -= new Vector3(GameManager.instance.background.levelWidth, 0f);
            GameObject rightMirage = Instantiate(graphics, graphics.transform.parent);
            rightMirage.transform.position += new Vector3(GameManager.instance.background.levelWidth, 0f);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (transform.position.x < GameManager.instance.background.leftEdge.transform.position.x)
        {
            transform.position += new Vector3(GameManager.instance.background.levelWidth, 0f);
            if (moveCamera) // move camera is for the player wrapping
                Camera.main.transform.position += new Vector3(GameManager.instance.background.levelWidth, 0f);
        }

        if (transform.position.x > GameManager.instance.background.rightEdge.transform.position.x)
        {
            transform.position -= new Vector3(GameManager.instance.background.levelWidth, 0f);
            if (moveCamera)
                Camera.main.transform.position -= new Vector3(GameManager.instance.background.levelWidth, 0f);
        }
    }
}
