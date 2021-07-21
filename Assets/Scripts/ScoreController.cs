using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    private static int Score = 0;
    private static TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        scoreText = GetComponent<TextMeshProUGUI>();
        scoreText.text = Score.ToString();
    }

    public static void ChangeScore(int amount)
    {
        Score += amount;
        scoreText.text = Score.ToString();
    }

    public static int GetScore()
    {
        return Score;
    }

}
