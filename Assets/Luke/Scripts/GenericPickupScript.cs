using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GenericPickupScript : MonoBehaviour
{
    [SerializeField] private AudioClip pickUpPing;

    [SerializeField] private UnityEvent onPickUp = default;
    private void OnTriggerEnter(Collider other)
    {
        onPickUp?.Invoke();
        FindObjectOfType<SoundController>().PlaySoundClipOneShot(pickUpPing);
        Destroy(this.gameObject);
    }
}
