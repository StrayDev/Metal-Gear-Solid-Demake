using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class KeycardScript : MonoBehaviour
{
    public enum KeycardColor : int
    {
        Undefined = 0,
        Blue,
        Red,
        Orange,
    }

    [SerializeField] private UnityEvent onPickUp = default;
    [SerializeField] private KeycardColor color = KeycardColor.Undefined;

    [SerializeField] private SpriteRenderer sprite;

    void Awake() {
        sprite.color = color switch {
            KeycardColor.Blue => Color.blue,
            KeycardColor.Red => Color.red,
            KeycardColor.Orange => new Color(0.255f * 6.0f, 0.165f * 6.0f, 0.0f),
            _ => throw new ArgumentException($"Unregistered colour used to set keycard colour: {color}")
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        onPickUp?.Invoke();
        Destroy(this.gameObject);
    }

    public KeycardColor GetKeycardColor() { return color; }
}
