using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5F;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer _renderer;

    private Rigidbody _rb = null;
    private Vector3 _direction = default;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 move, float moveMultiplier = 1f)
    {
        _direction = new Vector3(move.x, move.y, 0F);
        if (_direction.x > 0)
        {
            _renderer.flipX = true;
        }
        else if (_direction.x < 0)
        {
            _renderer.flipX = false;
        }
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        _rb.velocity = _direction.normalized * moveSpeed * moveMultiplier;
        if (_rb.velocity.x != 0 || _rb.velocity.y != 0)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }
    }

    public void Direction(Vector2 dir)
    {
        if (dir == Vector2.zero) return;
        _direction = dir; 
        transform.up = _direction;    
    }

    public void setMovementSpeed(float _movement) {
        moveSpeed = _movement;
    }

    public float MoveSpeed => moveSpeed;
}
