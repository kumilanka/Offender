using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject explosion;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnDestroy()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        SoundManager.instance.PlaySound("Explosion");
    }
}
