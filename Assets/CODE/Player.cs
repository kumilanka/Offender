using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public GameObject ship;
    public GameObject ammoPrefab;
    public Transform shootPosition;
    public float shootVelocity;
    private bool facingRight;
    public float maxHorizontalVelocity = 1f;
    public float maxVerticalVelocity = 1f;
    public float horizontalAcceleration = 0.05f;
    private float horizontalVelocity; // current horizontal velocity
    private Rigidbody2D rigidbody2d;

	void Start ()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        // normal shooting using space for now
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject ammo = Instantiate(ammoPrefab, shootPosition.position, Quaternion.identity);
            ammo.GetComponent<Rigidbody2D>().velocity = new Vector2(shootVelocity * (facingRight ? 1f : -1f), 0f);
            SoundManager.instance.PlaySound("Laser");
        }

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // vertical movement is linear on/off type, limited to the camera's viewport
        if (Input.GetKey(KeyCode.UpArrow) && viewportPosition.y < 0.9f)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, maxVerticalVelocity);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && viewportPosition.y > 0.1f)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -maxVerticalVelocity);
        }
        else
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0f);
        }

        //transform.position += new Vector3(horizontal, vertical, 0f) * maxHorizontalSpeed;

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
