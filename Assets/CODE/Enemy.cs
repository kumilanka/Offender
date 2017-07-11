using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject explosion;

    public void Kill()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        SoundManager.instance.PlaySound("Explosion");

        Destroy(gameObject);
    }
}
