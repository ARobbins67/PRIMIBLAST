using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI YourScore;

    [SerializeField] TextMeshProUGUI Score1Text;

    [SerializeField] TextMeshProUGUI Score2Text;

    [SerializeField] TextMeshProUGUI Score3Text;

    public List<int> Scores = new List<int>();

    private int currentScore;
    
    public void ShowEndScreen()
    {
        gameObject.SetActive(true);
        currentScore = ScoreController.GetScore();
        
        Scores.Clear();
        Scores.Add(0);
        Scores.Add(0);
        Scores.Add(0);
        
        // get current data
        PlayerData data = SaveSystem.GetData(Scores[0], Scores[1], Scores[2]);
        
        Scores.Add(data.Score1);
        Scores.Add(data.Score2);
        Scores.Add(data.Score3);
        
        UpdateScores();

        // save updated data
        SaveSystem.SavePlayer(this);

        UpdateUI();
    }

    private void UpdateScores()
    {
        Scores.Add(currentScore);
        Scores.Sort();
        Scores.Reverse();
        foreach (int score in Scores)
        {
            Debug.Log(score);
        }
        
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void UpdateUI()
    {
        YourScore.text = "Your Score: " + currentScore;

        if (Scores[0] > 0)
        {
            Score1Text.text = "1. " + Scores[0];
        }
        else
        {
            Score1Text.text = "1. ---";
        }

        if (Scores[1] > 0)
        {
            Score2Text.text = "2. " + Scores[1];
        }
        else
        {
            Score2Text.text = "2. ---";
        }

        if (Scores[2] > 0)
        {
            Score3Text.text = "3. " + Scores[2];
        }
        else
        {
            Score3Text.text = "3. ---";
        }
    }
}
