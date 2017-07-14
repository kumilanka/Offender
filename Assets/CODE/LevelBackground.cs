using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBackground : MonoBehaviour
{
    public GameObject starPrefab;

    public GameObject leftEdge;
    public GameObject rightEdge;
    public List<GameObject> backgroundPrefabs = new List<GameObject>();

    public GameObject world1;
    public GameObject world2;

    public float levelWidth
    {
        get
        {
            return Mathf.Abs(leftEdge.transform.position.x - rightEdge.transform.position.x);
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
        GameObject background = new GameObject("center");
        background.transform.SetParent(transform);

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
                    GameObject star = Instantiate(starPrefab, background.transform);
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
        GameObject ground = Instantiate(backgroundPrefabs.Find(x => x.name == "Ground"), background.transform);
        ground.transform.position = new Vector2(0f, -3f);

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
                    float yPos = groundTopPoint - (((float)i / gridHeight) * (groundTopPoint - groundBottomPoint));

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


        GameObject copyBackground = Instantiate(background, transform);
        copyBackground.name = "right";
        copyBackground.transform.localPosition = new Vector2(levelWidth, 0f);

        copyBackground = Instantiate(background, transform);
        copyBackground.name = "left";
        copyBackground.transform.localPosition = new Vector2(-levelWidth, 0f);

    }

    private GameObject GetRandomGroundClutterPrefab()
    {
        List<GameObject> groundClutter = backgroundPrefabs.FindAll(x => x.name != "Ground");
        return groundClutter[Random.Range(0, groundClutter.Count)];
    }
}
