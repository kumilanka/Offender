using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float lifetime = 3f;
    private float r_lifetime;

	void Start ()
    {
        r_lifetime = lifetime;
	}
	
	void Update ()
    {
        // destroy bullet after lifetime has expired
        r_lifetime -= Time.deltaTime;
        if (r_lifetime <= 0f)
            Destroy(gameObject);
	}

    public void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Enemy")
        {
            c.gameObject.GetComponent<Enemy>().Kill();
        }

        if (c.collider.gameObject.tag != "Player" && c.gameObject.tag != "Ammo")
            Destroy(gameObject);
    }
}
