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

    public LevelBackground background;

    [HideInInspector]
    public Player player;
    public GameObject playerPrefab;

    public GameObject alienPrefab;
    public GameObject wheelchairPrefab;

    public List<Level> levels = new List<Level>();

    private List<GameObject> wheelchairs = new List<GameObject>(); // list of all the wheelchairs in the level
    private List<GameObject> availableWheelchairs = new List<GameObject>(); // list of all the wheelchairs available for targeting

    private List<GameObject> aliens = new List<GameObject>();

    [HideInInspector]
    public bool game; // whether the game is running or not (to display title screen)

    private float gameTimer;
    public Level currentLevel;

    public int lives = 3;
    public int score = 0;

    public List<Tuple<string, int>> highscoreTable = new List<Tuple<string, int>>();

	void Awake ()
    {
        // pseudo-singleton for easy access
        instance = this;
	}

    void Start()
    {
        SaveManager.instance.Load(); // load saved settings (uses PlayerPrefs because lazy)
        UIManager.instance.UpdateInitialUIState();
        OnGameOver();
    }

    public void RegisterAlien(GameObject alien)
    {
        if (!aliens.Contains(alien))
            aliens.Add(alien);
    }

    public void UnregisterAlien(GameObject alien)
    {
        if (aliens.Contains(alien))
            aliens.Remove(alien);

        // if alien had a target, release it so others may give chase
        Enemy e = alien.GetComponent<Enemy>();
        if (e.target != null && e.target.GetComponent<Wheelchair>() != null)
            if (!availableWheelchairs.Contains(e.target.gameObject))
                availableWheelchairs.Add(e.target.gameObject);
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

                            GameObject alien = Instantiate(alienPrefab, new Vector2(xPos, yPos), Quaternion.identity);
                            RegisterAlien(alien);
                        }
                    }
                }
            }
        }

        
	}

    public float WrapDistance(Vector2 worldPos1, Vector2 worldPos2)
    {
        float normalDistance = Vector2.Distance(worldPos1, worldPos2);
        // return the shorter distance between the two points, with x wrapped around using the level width
        if (worldPos1.x > worldPos2.x)
        {
            float wrapAroundPosDistance = Vector2.Distance(new Vector2(worldPos1.x - background.levelWidth, worldPos1.y), worldPos2);
            return Mathf.Min(normalDistance, wrapAroundPosDistance);
        }
        else
        {
            float wrapAroundPosDistance = Vector2.Distance(new Vector2(worldPos1.x + background.levelWidth, worldPos1.y), worldPos2);
            return Mathf.Min(normalDistance, wrapAroundPosDistance);
        }
    }

    public Vector2 WrapDirection(Vector2 worldPos1, Vector2 worldPos2)
    {
        // return the wraparound direction from pos 1 toward pos 2, wrapped for x-axis around the level width

        // if the wraparound distance is bigger than the actual distance
        if (Vector2.Distance(worldPos1, worldPos2) <= WrapDistance(worldPos1, worldPos2))
        {
            Vector2 dir = worldPos2 - worldPos1;

            // just use the normal direction
            return dir;
        }
        else
        {

            Vector2 dir = worldPos1.x > worldPos2.x ?
                // wrap to the left
                worldPos2 - new Vector2(worldPos1.x - background.levelWidth, worldPos1.y) :
                // wrap to the right
                worldPos2 - new Vector2(worldPos1.x + background.levelWidth, worldPos1.y);

            // use the wraparound direction instead (since it's shorter)
            return dir;
        }
    }

    // utility function to wrap the world x coordinates around the level
    public Vector2 WrapX(Vector2 worldPos)
    {
        float leftPos = background.leftEdge.transform.position.x;
        return new Vector2(leftPos + Mathf.Repeat(worldPos.x, background.levelWidth), worldPos.y);
    }

    // call to return to title screen
    public void OnGameOver()
    {
        // clean up the lists of aliens and wheelchairs for the next play
        foreach (GameObject alien in aliens)
        {
            Destroy(alien);
        }
        aliens.Clear();

        foreach (GameObject wheelchair in wheelchairs)
        {
            Destroy(wheelchair);
        }

        wheelchairs.Clear();
        availableWheelchairs.Clear();

        game = false;
        SoundManager.instance.PlayMusic("IntroMusic");

        if (IsHighScore(score))
        {
            UIManager.instance.SetState(UIManager.UIStates.NameInput);
        }
        else
            UIManager.instance.SetState(UIManager.UIStates.TitleRotation);
    }

    public bool IsHighScore(int score)
    {
        foreach (Tuple<string, int> highscore in highscoreTable)
        {
            if (score > highscore.two) // only allow highscore that is bigger than the existing one, got the same one? Too bad, someone got there first.
                return true;
        }

        return false;
    }

    public void RecordHighScore(string name, int score)
    {
        for (int i = 0; i < highscoreTable.Count; i++)
        {
            if (score > highscoreTable[i].two)
            {
                // insert at the first place where the score is higher than current, bumping everything down by one
                highscoreTable.Insert(i, new Tuple<string, int>(name, score));
                // then remove the last one on the list
                highscoreTable.RemoveAt(highscoreTable.Count - 1);

                SaveManager.instance.Save();

                return;
            }
        }
    }

    public void OnStartGameButtonPressed()
    {
        // press any key to start
        game = true;
        

        SoundManager.instance.StopMusic();
        SoundManager.instance.PlaySound("Weird");

        UIManager.instance.SetState(UIManager.UIStates.Game);

        StartNewGame();
    }

    public void OnPlayerDeath()
    {
        lives--;
        if (lives < 1)
            OnGameOver();
        else
            SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        Instantiate(playerPrefab);
    }

    public void StartNewGame()
    {
        SpawnWheelchairs();
        gameTimer = 0f;
        lives = 3;
        score = 0;

        currentLevel = levels[0];
        SpawnPlayer();
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
            availableWheelchairs.Sort((x, y) => WrapDistance(position, x.transform.position) < WrapDistance(position, y.transform.position) ? -1 : 1);

            GameObject wheelchair = availableWheelchairs[0];

            if (remove)
                availableWheelchairs.Remove(wheelchair);
            return wheelchair;
        }

        Debug.Log("No wheelchairs available right now!");

        return null;
    }


}
