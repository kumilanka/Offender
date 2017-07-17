using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheelchair : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        StartCoroutine(MoveAround());	
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator MoveAround()
    {
        Vector3 targetPos = transform.position;
        targetPos.x += Random.Range(-0.5f, 0.5f);

        while (Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            GetComponent<Rigidbody2D>().velocity = targetPos - transform.position;
            yield return null;
        }

        float randomTime = Random.Range(1f, 4f);

        yield return new WaitForSeconds(randomTime);

        StartCoroutine(MoveAround());
    }
}
