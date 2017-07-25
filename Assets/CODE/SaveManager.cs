using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    void Awake()
    {
        instance = this;
    }

    public void Load()
    {
        // sound mute status
        SoundManager.instance.Mute = PlayerPrefs.GetInt(string.Format("{0}{1}", savePrefix, "mute"), 0) == 1 ? true : false;

        GameManager.instance.highscoreTable.Clear();
        // this is bad design right here, the number of scores on the scores list should be defined by a single variable somewhere, and not by some random list.
        for (int i = 0; i < UIManager.instance.scoreElements.Count; i++)
        {
            int scoreIndex = i;
            int score = PlayerPrefs.GetInt(string.Format("{0}{1}{2}", savePrefix, "highscoreValue", scoreIndex), 0);
            string name = PlayerPrefs.GetString(string.Format("{0}{1}{2}", savePrefix, "highscoreName", scoreIndex), "SJW");
            GameManager.instance.highscoreTable.Add(new Tuple<string, int>(name, score));
        }
    }

    public void Save()
    {
        // save the mute setting
        PlayerPrefs.SetInt(string.Format("{0}{1}", savePrefix, "mute"), SoundManager.instance.Mute ? 1 : 0);

        // save the highscore table using indexed settings for the score and the name
        for (int i = 0; i < UIManager.instance.scoreElements.Count; i++)
        {
            int scoreIndex = i;
            int score = i < GameManager.instance.highscoreTable.Count ? GameManager.instance.highscoreTable[i].two : 0;
            string name = i < GameManager.instance.highscoreTable.Count ? GameManager.instance.highscoreTable[i].one : "SJW";
            PlayerPrefs.SetInt(string.Format("{0}{1}{2}", savePrefix, "highscoreValue", scoreIndex), score);
            PlayerPrefs.SetString(string.Format("{0}{1}{2}", savePrefix, "highscoreName", scoreIndex), name);
        }
    }

    public const string savePrefix = "Offender_";


}
