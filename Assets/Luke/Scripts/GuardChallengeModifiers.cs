using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardChallengeModifiers : MonoBehaviour
{
    [SerializeField] private SimpleGuardBrain brain;
    [SerializeField] private MoveComponent moveComponent;

    [SerializeField] private float difficulty_2_speed_increase = 1f;
    [SerializeField] private float difficulty_4_speed_increase = 2f;

    private int difficulty;

    void Start()
    {
        difficulty = brain.GetLevelController.getDifficultyLevel();
        moveComponent.setMovementSpeed((difficulty >= 2 ? (difficulty == 4 ? moveComponent.MoveSpeed+difficulty_4_speed_increase : moveComponent.MoveSpeed+difficulty_2_speed_increase ) : moveComponent.MoveSpeed));
    }
}
