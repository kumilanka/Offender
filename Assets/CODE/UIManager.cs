using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Button soundButton;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    public GameObject startScreen;

    public List<GameObject> lifeObjects = new List<GameObject>();
    public Text scoreLabel;

    public GameObject gameUI;

    public List<ScoreElement> scoreElements = new List<ScoreElement>();

    public List<GameObject> rotatingTitleScreens = new List<GameObject>();

    public GameObject nameDisplay;

    public InputField highscoreNameField;
    public Text highscoreScoreLabel;
    public Text displayMessageLabel; // for displaying messages like Get Ready or something in the middle of the screen during gameplay

    private float displayMessageTimer;

    [System.NonSerialized]
    public GameObject currentRotatingScreen;
    public float rotationTime = 5f;
    private float r_rotationTime;

    public enum UIStates
    {
        TitleRotation,
        Game,
        NameInput
    }

    [System.NonSerialized]
    public UIStates state;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (GameObject go in rotatingTitleScreens)
            go.SetActive(false);

        r_rotationTime = rotationTime;
        currentRotatingScreen = rotatingTitleScreens[0];
    }

    public void DisplayMessage(string text, float time = 2f)
    {
        displayMessageTimer = time;
        displayMessageLabel.text = text;
    }

    public void UpdateInitialUIState()
    {
        soundButton.image.sprite = SoundManager.instance.Mute ? soundOffSprite : soundOnSprite;
    }

    void Update()
    {
        switch (state)
        {
            case UIStates.Game:
                {
                    UpdateGameUI();
                    gameUI.SetActive(true);
                    startScreen.SetActive(false);

                    // update display message via timer
                    displayMessageTimer -= Time.deltaTime;
                    if (displayMessageTimer <= 0f)
                    {
                        displayMessageLabel.gameObject.SetActive(false);
                    }
                    else
                    {
                        displayMessageLabel.gameObject.SetActive(true);
                    }
                }
                break;

            case UIStates.TitleRotation:
                {
                    gameUI.SetActive(false);
                    startScreen.SetActive(true);
                    nameDisplay.SetActive(false);

                    if (currentRotatingScreen != null && !currentRotatingScreen.activeInHierarchy)
                        currentRotatingScreen.SetActive(true);

                    r_rotationTime -= Time.deltaTime;
                    if (r_rotationTime <= 0f)
                    {
                        r_rotationTime = rotationTime;

                        currentRotatingScreen.SetActive(false);
                        int index = rotatingTitleScreens.IndexOf(currentRotatingScreen);
                        if (++index > rotatingTitleScreens.Count - 1)
                            index = 0;

                        currentRotatingScreen = rotatingTitleScreens[index];
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                        GameManager.instance.OnStartGameButtonPressed();
                }
                break;

            case UIStates.NameInput:
                {
                    highscoreScoreLabel.text = string.Format("{0}", GameManager.instance.score);
                    gameUI.SetActive(false);
                    startScreen.SetActive(true);
                    if (currentRotatingScreen != null)
                        currentRotatingScreen.SetActive(false);

                    nameDisplay.SetActive(true);
                }
                break;

            default:
                break;
        }

    }

    public void OnPlayerNameEdited(InputField field)
    {
        StartCoroutine(RecordHighScore(field.text, GameManager.instance.score));
    }

    private IEnumerator RecordHighScore(string name, int score)
    {
        yield return null;

        GameManager.instance.RecordHighScore(name, GameManager.instance.score);
        SetState(UIStates.TitleRotation);
        r_rotationTime = rotationTime * 2f;
        if (currentRotatingScreen != null)
            currentRotatingScreen.SetActive(false);

        currentRotatingScreen = rotatingTitleScreens[1]; // dirty hack to display the highscore table next, then continue with the title screen rotation
    }


    public void SetState(UIStates newState)
    {
        state = newState;

        if (newState == UIStates.TitleRotation)
            UpdateHighScoreDisplay(GameManager.instance.highscoreTable);

        if (newState == UIStates.NameInput)
        {
            StartCoroutine(InputHack());
        }
    }

    private IEnumerator InputHack()
    {
        yield return null;
        highscoreNameField.ActivateInputField();
        highscoreNameField.Select();
    }

    public void UpdateHighScoreDisplay(List<Tuple<string, int>> highscore)
    {
        for (int i = 0; i < scoreElements.Count; i++)
        {
            if (i < highscore.Count)
            {
                scoreElements[i].SetScore(highscore[i].one, highscore[i].two);
            }
        }
    }

    public void DisplayMenu()
    {
        startScreen.SetActive(true);
        gameUI.SetActive(false);

        UpdateHighScoreDisplay(GameManager.instance.highscoreTable);
    }

    public void DisplayGameUI()
    {
        startScreen.SetActive(false);
        gameUI.SetActive(true);
    }

    public void OnSoundButtonPressed()
    {
        // toggle the mute flag
        SoundManager.instance.Mute = !SoundManager.instance.Mute;

        SaveManager.instance.Save(); // save changes to mute

        // update the button graphic
        if (soundButton != null)
        {
            soundButton.image.sprite = SoundManager.instance.Mute ? soundOffSprite : soundOnSprite;
        }

        
    }

    /// <summary>
    /// Updates the game time ui which is currently just the lives display and the score display
    /// </summary>
    private void UpdateGameUI()
    {
        foreach (GameObject lifeObject in lifeObjects)
        {
            if (GameManager.instance.lives > lifeObjects.IndexOf(lifeObject))
                lifeObject.SetActive(true);
            else
                lifeObject.SetActive(false);
        }

        scoreLabel.text = string.Format("{0}", GameManager.instance.score);
    }
}
