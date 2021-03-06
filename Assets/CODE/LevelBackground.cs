﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBackground : MonoBehaviour
{
    public List<GameObject> starPrefabs = new List<GameObject>();

    public GameObject leftEdge;
    public GameObject rightEdge;
    public List<GameObject> backgroundPrefabs = new List<GameObject>();

    [HideInInspector]
    public GameObject center;
    [HideInInspector]
    public GameObject left;
    [HideInInspector]
    public GameObject right;
    [HideInInspector]
    public GameObject ground;

    public float levelWidth
    {
        get
        {
            return Mathf.Abs(leftEdge.transform.position.x - rightEdge.transform.position.x);
        }
    }

    public Rect groundRect
    {
        get
        {
            SpriteRenderer spr = ground.GetComponent<SpriteRenderer>();
            return new Rect(spr.bounds.center - spr.bounds.extents, ground.GetComponent<SpriteRenderer>().bounds.size);
        }
    }

    // Use this for initialization
    void Start ()
    {
        CreateStaticBackground();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateStaticBackground()
    {
        int gridHeight = 20;
        int gridWidth = 80;

        float leftEdgePoint = leftEdge.transform.position.x;
        float rightEdgePoint = rightEdge.transform.position.x;
        float topEdgePoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)).y;
        float bottomEdgePoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).y;

        // build the background stuff procedurally under a common root object
        center = new GameObject("center");
        center.transform.SetParent(transform);

        // use perlin noise to generate some stars
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                if (Mathf.PerlinNoise(i * 20.1f, j * 20.1f) > 0.7f)
                {
                    float xPos = leftEdgePoint - (((float)j / gridWidth) * (leftEdgePoint - rightEdgePoint));
                    float yPos = topEdgePoint - (((float)i / gridHeight) * (topEdgePoint - bottomEdgePoint));

                    xPos += Random.Range(-1f, 1f);
                    yPos += Random.Range(-1f, 1f);
                    GameObject star = Instantiate(GetRandomStarPrefab(), center.transform);
                    //GameObject star = Instantiate(starPrefab, new Vector2(xPos, yPos), Quaternion.identity);
                    star.transform.position = new Vector2(xPos, yPos);
                    FlashySprite fs = star.GetComponent<FlashySprite>();
                    fs.colorCycleTime = Random.Range(0.1f, 0.3f);
                    fs.colorCycleStart = Random.Range(0f, fs.colorCycleTime);
                    fs.alphaCycleTime = Random.Range(2f, 5f);
                    fs.alphaCycleStart = Random.Range(0f, fs.alphaCycleTime);

                    star.transform.localScale = star.transform.localScale * Random.Range(0.8f, 1.2f);
                }
            }
        }

        // generate a ground object using a tiled sprite
        ground = Instantiate(backgroundPrefabs.Find(x => x.name == "Ground"), center.transform);
        ground.GetComponent<SpriteRenderer>().size = new Vector2(levelWidth, ground.GetComponent<SpriteRenderer>().size.y);

        ground.transform.position = new Vector2(0f, Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0f)).y);

        float groundTopPoint = ground.transform.position.y + ground.GetComponent<SpriteRenderer>().bounds.extents.y;
        float groundBottomPoint = ground.transform.position.y - ground.GetComponent<SpriteRenderer>().bounds.extents.y;

        gridHeight = 10;
        gridWidth = 40;

        // use perlin noise to generate some ground clutter
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                if (Mathf.PerlinNoise(i * 20.1f, j * 20.1f) > 0.8f)
                {

                    float xPos = leftEdgePoint - (((float)j / gridWidth) * (leftEdgePoint - rightEdgePoint));
                    float yPos = groundRect.yMax - (((float)i / gridHeight) * (groundRect.yMax - groundRect.yMin));

                    xPos += Random.Range(-0.1f, 0.1f);
                    //yPos += Random.Range(-1f, 1f);

                    GameObject randomPrefab = GetRandomGroundClutterPrefab();

                    GameObject star = Instantiate(randomPrefab, ground.transform);
                    //GameObject star = Instantiate(starPrefab, new Vector2(xPos, yPos), Quaternion.identity);
                    star.transform.position = new Vector2(xPos, yPos);
                    //FlashySprite fs = star.GetComponent<FlashySprite>();
                    //fs.colorCycleTime = Random.Range(0.1f, 0.3f);
                    //fs.colorCycleStart = Random.Range(0f, fs.colorCycleTime);
                    //fs.alphaCycleTime = Random.Range(2f, 5f);
                    //fs.alphaCycleStart = Random.Range(0f, fs.alphaCycleTime);

                    star.transform.localScale = star.transform.localScale * Random.Range(0.8f, 1.2f);
                }
            }
        }


        right = Instantiate(center, transform);
        right.name = "right";
        right.transform.localPosition = new Vector2(levelWidth, 0f);

        left = Instantiate(center, transform);
        left.name = "left";
        left.transform.localPosition = new Vector2(-levelWidth, 0f);

    }

    private GameObject GetRandomStarPrefab()
    {
        return starPrefabs[Random.Range(0, starPrefabs.Count)];
    }

    private GameObject GetRandomGroundClutterPrefab()
    {
        List<GameObject> groundClutter = backgroundPrefabs.FindAll(x => x.name != "Ground");
        return groundClutter[Random.Range(0, groundClutter.Count)];
    }
}
