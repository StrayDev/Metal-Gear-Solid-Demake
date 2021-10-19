using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class KeycardScript : MonoBehaviour
{
    [SerializeField] private UnityEvent onPickUp = default;

    private void OnTriggerEnter(Collider other)
    {
        onPickUp?.Invoke();
        Destroy(this.gameObject);
    }
}
