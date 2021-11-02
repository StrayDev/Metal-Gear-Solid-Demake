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
        Green,
        Yellow,
        Purple
    }

    [SerializeField] private AudioClip pickUpPing;

    [SerializeField] private UnityEvent onPickUp = default;
    [SerializeField] private KeycardColor color = KeycardColor.Undefined;

    [SerializeField] private SpriteRenderer sprite;

    private SoundController soundController;

    void Awake() {
        soundController = FindObjectOfType<SoundController>();
        sprite.color = color switch {
            KeycardColor.Blue => Color.blue,
            KeycardColor.Red => Color.red,
            KeycardColor.Orange => new Color(1f, 173f/255f, 0.0f),
            KeycardColor.Green => Color.green,
            KeycardColor.Yellow => Color.yellow,
            KeycardColor.Purple => new Color(123,0,192),
            _ => throw new ArgumentException($"Unregistered colour used to set keycard colour: {color}")
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        onPickUp?.Invoke();
        soundController?.PlaySoundClipOneShot(pickUpPing);
        Destroy(this.gameObject);
    }

    public KeycardColor GetKeycardColor() { return color; }
}
