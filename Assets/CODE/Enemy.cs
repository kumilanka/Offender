using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes
{
    Alien
}

public class Enemy : MonoBehaviour
{
    public GameObject explosion;

    [System.NonSerialized]
    public Transform target;

    public float normalSpeed = 2f;

    void Update()
    {
        // let's see here, enemy AI...

        // for the basic alien, they start at the top, and try to move toward the wheelchairs at the bottom to abduct them (collide to abduct) and then return to the top to complete abduction

        // so they have two modes, abduction mode that has two phases, go toward wheelchair and then go to the top
        // the second mode is the mutant mode, where they go crazy and start chasing the player at high speed

        if (target == null)
        {
            FindTarget();
        }
        else
        {
            Vector3 dir = target.position - transform.position;

            GetComponent<Rigidbody2D>().velocity = dir.normalized * normalSpeed;
        }
    }

    private void FindTarget()
    {
        GameObject wheelchair = GameManager.instance.GetClosestAvailableWheelchair(transform.position, true);

        if (wheelchair == null)
        {
            // no wheelchairs available, go for the player instead
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            target = wheelchair.transform;
        }
    }



    public void Kill()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        SoundManager.instance.PlaySound("Explosion");

        Destroy(gameObject);
    }
}
