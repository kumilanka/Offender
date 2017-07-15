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

    public LevelBackground background;

    public Player player;

    public GameObject alienPrefab;
    public GameObject wheelchairPrefab;

    public List<Level> levels = new List<Level>();

    private List<GameObject> wheelchairs = new List<GameObject>(); // list of all the wheelchairs in the level
    private List<GameObject> availableWheelchairs = new List<GameObject>(); // list of all the wheelchairs available for targeting


    public GameObject startScreen;
    [HideInInspector]
    public bool game; // whether the game is running or not (to display title screen)

    private float gameTimer;
    public Level currentLevel;

	void Awake ()
    {
        // pseudo-singleton for easy access
        instance = this;
	}

    void Start()
    {
        OnGameOver();
        
    }

    
	
	void Update ()
    {
        if (game)
        {
            gameTimer += Time.deltaTime;

            foreach (Wave w in currentLevel.waves)
            {
                if (gameTimer > w.spawnTime && !w.spawned)
                {
                    w.spawned = true;
                    Debug.Log("Spawning a new wave of enemies!");

                    Rect groundRect = background.groundRect;

                    for (int i = 0; i < w.Enemies.Count; i++)
                    {
                        EnemyTypes type = w.Enemies[i];

                        if (type == EnemyTypes.Alien)
                        {
                            // place the aliens at the top so they can start to hunt the wheelchairs (and the player too)
                            float single = groundRect.width / w.Enemies.Count;

                            float xPos = groundRect.xMin + single / 2f + (single * i);
                            float yPos = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0.9f)).y;

                            Instantiate(alienPrefab, new Vector2(xPos, yPos), Quaternion.identity);
                        }
                    }
                }
            }
        }
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

        StartNewGame();
    }

    public void StartNewGame()
    {
        SpawnWheelchairs();
        gameTimer = 0f;

        currentLevel = levels[0];
    }

    public void SpawnWheelchairs()
    {
        Rect groundRect = background.groundRect;

        int amount = 8;

        // spawn the wheelchairs for the level (the civilians that the aliens are trying to abduct)
        for (int i = 0; i < amount; i++)
        {
            // place the wheelchairs relatively evenly on the ground
            float single = groundRect.width / amount;

            float xPos = groundRect.xMin + single / 2f + (single * i);
            float yPos = groundRect.center.y;

            GameObject wheelchair = Instantiate(wheelchairPrefab, new Vector2(xPos, yPos), Quaternion.identity);

            wheelchairs.Add(wheelchair);
            availableWheelchairs.Add(wheelchair);
        }
    }

    public GameObject GetClosestAvailableWheelchair(Vector2 position, bool remove)
    {
        

        if (availableWheelchairs.Count > 0)
        {
            availableWheelchairs.Sort((x, y) => Vector2.Distance(position, x.transform.position) < Vector2.Distance(position, y.transform.position) ? -1 : 1);

            GameObject wheelchair = availableWheelchairs[0];

            if (remove)
                availableWheelchairs.Remove(wheelchair);
            return wheelchair;
        }

        Debug.Log("No wheelchairs available right now!");

        return null;
    }
}
