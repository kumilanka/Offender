using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlipByVelocity : MonoBehaviour
{
    public GameObject graphics;
    private Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((graphics.transform.localScale.x > 0f && rb2d.velocity.x > 0f)
            || (graphics.transform.localScale.x < 0f && rb2d.velocity.x < 0f))
        {
            Vector3 newLocalScale = graphics.transform.localScale;
            newLocalScale.x *= -1;

            graphics.transform.localScale = newLocalScale;
        }
    }
}
