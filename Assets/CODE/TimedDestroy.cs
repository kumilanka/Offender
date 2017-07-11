using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public float lifeTime = 3f;
    private float r_lifeTime;

	void Start ()
    {
        r_lifeTime = lifeTime;
	}
	
	void Update ()
    {
        r_lifeTime -= Time.deltaTime;
        if (r_lifeTime <= 0f)
            Destroy(gameObject);
	}
}
