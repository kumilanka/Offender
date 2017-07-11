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
