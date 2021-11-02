using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private int score;

    private void Update()
    {
        score = int.Parse(scoreText.text);
        ScoreSingleton.Instance.val = score;
    }

    public void AddScore200()
    {
        score += 200;
        scoreText.text = score.ToString();
    }
    public void AddScore1000()
    {
        score += 1000;
        scoreText.text = score.ToString();
    }
}
