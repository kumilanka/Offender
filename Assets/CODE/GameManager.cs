using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager class to handle all the general gameplay stuff such as starting of the game, keeping score etc.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject startScreen;
    [HideInInspector]
    public bool game; // whether the game is running or not (to display title screen)

	void Start ()
    {
        // pseudo-singleton for easy access
        instance = this;
	}
	
	void Update ()
    {
        if (!game)
        {
            startScreen.SetActive(true);
            if (Input.anyKey)
                game = true;
        }

        else
            startScreen.SetActive(false);	
	}
}
