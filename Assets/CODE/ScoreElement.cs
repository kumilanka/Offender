using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreElement : MonoBehaviour
{
    public Text scoreText;
    public Text nameText;

    public void SetScore(string name, int score)
    {
        scoreText.text = string.Format("{0}", score);
        nameText.text = name;
    }
}
