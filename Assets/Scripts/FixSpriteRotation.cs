using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSpriteRotation : MonoBehaviour
{
    private Transform _t = null;

    private void Start()
    {
        _t = transform;
    }

    private void Update()
    {
        transform.up = Vector3.up;
        transform.right = Vector3.right;
    }
}
