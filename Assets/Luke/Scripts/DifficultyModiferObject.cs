using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class DifficultyModiferObject
{
    [SerializeField] private GameObject target_object;
    [SerializeField] private int target_difficulty_level;

    public GameObject targetObject => target_object;
    public int targetDifficultyLevel => target_difficulty_level;

    DifficultyModiferObject(GameObject _target_obj, int _target_difficulty_level) { target_object = _target_obj; target_difficulty_level = _target_difficulty_level; }
}