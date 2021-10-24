using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5F;

    private Rigidbody _rb = null;
    private Vector3 _direction = default;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 move, float moveMultiplier = 1f)
    {
        _direction = new Vector3(move.x, move.y, 0F);
        _rb.velocity = _direction.normalized * moveSpeed * moveMultiplier;
    }

    public void Direction(Vector2 dir)
    {
        if (dir == Vector2.zero) return;
        _direction = dir; 
        transform.up = _direction;    
    }
}
