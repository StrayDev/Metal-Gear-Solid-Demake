using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VentScript : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onInteractVent = default;

    public void Interact() { 
        onInteractVent?.Invoke();
    }

}
