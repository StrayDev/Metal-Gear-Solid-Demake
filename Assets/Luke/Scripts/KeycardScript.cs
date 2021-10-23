using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class KeycardScript : MonoBehaviour
{
    public enum KeycardColor : int
    {
        Undefined = 0,
        Blue,
        Orange,
    }

    [SerializeField] private UnityEvent onPickUp = default;
    [SerializeField] private KeycardColor color = KeycardColor.Undefined;

    private void OnTriggerEnter(Collider other)
    {
        onPickUp?.Invoke();
        Destroy(this.gameObject);
    }

    public KeycardColor GetKeycardColor() { return color; }
}
