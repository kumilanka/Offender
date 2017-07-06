using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ship;
    public GameObject ammoPrefab;
    public Transform shootPosition;
    public float shootVelocity;
    private bool facingRight;
    public float movementSpeed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject ammo = Instantiate(ammoPrefab, shootPosition.position, Quaternion.identity);
            ammo.GetComponent<Rigidbody2D>().velocity = new Vector2(shootVelocity * (facingRight ? 1f : -1f), 0f);
        }

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.position += new Vector3(horizontal, vertical, 0f) * movementSpeed;

        if (horizontal > 0f && !facingRight)
        {
            facingRight = true;
            ship.transform.localScale = new Vector3(ship.transform.localScale.x * -1f, ship.transform.localScale.y, ship.transform.localScale.z);
        }

        else if (horizontal < 0f && facingRight)
        {
            facingRight = false;
            ship.transform.localScale = new Vector3(ship.transform.localScale.x * -1f, ship.transform.localScale.y, ship.transform.localScale.z);
        }
	}
}
