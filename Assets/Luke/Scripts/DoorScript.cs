using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorScript : MonoBehaviour, IInteractable
{
    [SerializeField] private bool locked = true;
    [SerializeField] private bool multiUse = false;
    public bool open {get; private set;} = false;

    [SerializeField] private UnityEvent onInteractDoorUnlocked = default;
    [SerializeField] private UnityEvent onInteractDoorLocked = default;

    [SerializeField] private Transform teleport_location;

    [SerializeField] private LevelController level_controller;

    public void Unlock() {
        locked = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
    public void Interact() {
        if (!locked && (!open || multiUse)) {
            onInteractDoorUnlocked?.Invoke();
            TempOpenDoorFnc();
            return;
        }
        onInteractDoorLocked?.Invoke();
    }

    private void TempOpenDoorFnc()
    {
        //transform.position += (transform.right*2);
        open = true;
        level_controller.VentPlayerToLoaction(teleport_location);
    }

}

