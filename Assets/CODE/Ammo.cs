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
}
