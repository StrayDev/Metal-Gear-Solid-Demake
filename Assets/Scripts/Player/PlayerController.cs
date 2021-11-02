using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private Animator anim;

    private const string Hor = "Horizontal";
    private const string Ver = "Vertical";

    private Rigidbody _rb;
    private Vector2 _direction = Vector2.up;
    
    private MoveComponent _moveComp = null;
    private InteractComponent _interactComp = null;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _moveComp = GetComponent<MoveComponent>();
        _interactComp = GetComponent<InteractComponent>();
    }

    private void Update()
    {
        UpdateMoveAndDirection();
        CheckButtonPress();
    }

    private void UpdateMoveAndDirection()
    {
        var input = new Vector2(Input.GetAxisRaw(Hor), Input.GetAxisRaw(Ver));
        _moveComp.Move(input);
        _moveComp.Direction(input);
    }
    
    private void CheckButtonPress()
    {
        if (!Input.GetKeyDown(interactKey)) return;
        _interactComp.Interact();
        Debug.Log("Press");
    }
    
}