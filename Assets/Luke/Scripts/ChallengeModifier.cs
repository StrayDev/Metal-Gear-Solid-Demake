using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeModifier : MonoBehaviour
{
    [SerializeField] private int current_game_difficulty = 0;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public int currentGameDifficulty => current_game_difficulty;
    public void increase() {current_game_difficulty += 1;}
    public void reset() {current_game_difficulty = 0;}
}