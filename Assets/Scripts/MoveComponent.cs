using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5F;

    private Rigidbody _rb = null;
    private Vector3 _direction = default;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public void Move(Vector2 move)
    {
        _direction = new Vector3(move.x, move.y, 0F);
        _rb.velocity = _direction.normalized * moveSpeed;
    }

    public void Direction(Vector2 dir)
    {
        if (dir == Vector2.zero) return;
        _direction = dir; 
        transform.forward = _direction;    }
}