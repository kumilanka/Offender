using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameManager class to handle all the general gameplay stuff such as starting of the game, keeping score etc.
/// edit: Also it seems this class is now handling the UI stuff
/// todo: move UI stuff to a separate manager class if it grows out of hand :D
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Button soundButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    public GameObject leftEdge;
    public GameObject rightEdge;

    public GameObject starPrefab;

    public GameObject startScreen;
    [HideInInspector]
    public bool game; // whether the game is running or not (to display title screen)

	void Awake ()
    {
        // pseudo-singleton for easy access
        instance = this;
	}

    void Start()
    {
        OnGameOver();
        CreateStarField();
    }

    private void CreateStarField()
    {
        int gridHeight = 20;
        int gridWidth = 80;

        float leftEdgePoint = leftEdge.transform.position.x;
        float rightEdgePoint = rightEdge.transform.position.x;
        float topEdgePoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)).y;
        float bottomEdgePoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).y;

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
                    GameObject star = Instantiate(starPrefab, new Vector2(xPos, yPos), Quaternion.identity);
                    FlashySprite fs = star.GetComponent<FlashySprite>();
                    fs.colorCycleTime = Random.Range(0.1f, 0.3f);
                    fs.colorCycleStart = Random.Range(0f, fs.colorCycleTime);
                    fs.alphaCycleTime = Random.Range(2f, 5f);
                    fs.alphaCycleStart = Random.Range(0f, fs.alphaCycleTime);

                    star.transform.localScale = star.transform.localScale * Random.Range(0.8f, 1.2f);
                }
            }
        }
        
    }
	
	void Update ()
    {

	}

    public void OnSoundButtonPressed()
    {
        // toggle the mute flag
        SoundManager.instance.Mute = !SoundManager.instance.Mute;

        // update the button graphic
        if (soundButton != null)
        {
            soundButton.image.sprite = SoundManager.instance.Mute ? soundOffSprite : soundOnSprite;
        }
    }

    // call to return to title screen
    public void OnGameOver()
    {
        game = false;
        SoundManager.instance.PlayMusic("IntroMusic");
        startScreen.SetActive(true);
    }

    public void OnStartGameButtonPressed()
    {
        // press any key to start
        game = true;

        SoundManager.instance.StopMusic();
        SoundManager.instance.PlaySound("Weird");

        startScreen.SetActive(false);
    }
}
