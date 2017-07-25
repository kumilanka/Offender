using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public GameObject ship;
    public GameObject ammoPrefab;
    public Transform shootPosition;
    public Transform cameraFollowPosition;
    public float shootVelocity;
    private bool facingRight
    {
        get
        {
            // moved this to the flip by velocity component, which handles the turning of the graphic (set x scale to negative) by x velocity
            FlipByVelocity fbv = GetComponent<FlipByVelocity>();
            if (fbv != null)
                return GetComponent<FlipByVelocity>().graphics.transform.localScale.x < 0f;
            return false;
        }
    }
    public float maxHorizontalVelocity = 1f;
    public float maxVerticalVelocity = 1f;
    public float horizontalAcceleration = 0.05f;
    public float horizontalDrag = 0.01f;
    private float horizontalVelocity; // current horizontal velocity
    private Rigidbody2D rigidbody2d;

	void Start ()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        horizontalVelocity = 0f;
	}

    void Update()
    {
        // normal shooting using space for now
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject ammo = Instantiate(ammoPrefab, shootPosition.position, Quaternion.identity);
            ammo.GetComponent<Rigidbody2D>().velocity = new Vector2(rigidbody2d.velocity.x + shootVelocity * (facingRight ? 1f : -1f), 0f);
            SoundManager.instance.PlaySound("Laser");
        }
    }

    public void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Enemy")
        {
            c.gameObject.GetComponent<Enemy>().Kill();
            Destroy(gameObject);
            GameManager.instance.OnPlayerDeath();
        }

        //if (c.collider.gameObject.tag != "Player" && c.gameObject.tag != "Ammo")
            
    }

    void FixedUpdate ()
    {
        

        float horizontal = rigidbody2d.velocity.x;
        float vertical = 0f;

        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        // vertical movement is linear on/off type, limited to the camera's viewport
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && viewportPosition.y < 0.9f)
        {
            vertical = maxVerticalVelocity;
            //rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, maxVerticalVelocity);
        }
        else if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && viewportPosition.y > 0.1f)
        {
            vertical = -maxVerticalVelocity;
            //rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -maxVerticalVelocity);
        }
        //else
        //{
        //    vertical = 0f;
        //    //rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0f);
        //}

        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            if (horizontalVelocity <= maxHorizontalVelocity)
            {
                horizontalVelocity += horizontalAcceleration * Time.deltaTime;
            }
            horizontalVelocity = Mathf.Clamp(horizontalVelocity, -maxHorizontalVelocity, maxHorizontalVelocity);
        }
        else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            if (horizontalVelocity >= -maxHorizontalVelocity)
            {
                horizontalVelocity -= horizontalAcceleration * Time.deltaTime;
            }
            horizontalVelocity = Mathf.Clamp(horizontalVelocity, -maxHorizontalVelocity, maxHorizontalVelocity);
        }
        else
        {
            if (horizontalVelocity > 0f)
            {
                horizontalVelocity -= horizontalDrag * Time.deltaTime;
                if (horizontalVelocity < 0f)
                    horizontalVelocity = 0f;
            }
            if (horizontalVelocity < 0f)
            {
                horizontalVelocity += horizontalDrag * Time.deltaTime;
                if (horizontalVelocity > 0f)
                    horizontalVelocity = 0f;
            }
            //horizontalVelocity = Mathf.Lerp(horizontalVelocity, 0f, Time.deltaTime);
            //horizontalVelocity = Mathf.SmoothDamp(rigidbody2d.velocity.x, 0f, ref horizontalVelocity, horizontalDrag);
        }

        rigidbody2d.velocity = new Vector2(horizontalVelocity, vertical);

        //transform.position += new Vector3(horizontal, vertical, 0f) * maxHorizontalSpeed;

        //if (horizontal > 0f && !facingRight)
        //{
        //    facingRight = true;
        //    ship.transform.localScale = new Vector3(ship.transform.localScale.x * -1f, ship.transform.localScale.y, ship.transform.localScale.z);
        //}

        //else if (horizontal < 0f && facingRight)
        //{
        //    facingRight = false;
        //    ship.transform.localScale = new Vector3(ship.transform.localScale.x * -1f, ship.transform.localScale.y, ship.transform.localScale.z);
        //}
    }
}
