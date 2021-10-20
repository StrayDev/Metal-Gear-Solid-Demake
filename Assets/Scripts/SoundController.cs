using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource = null;

    public void PlaySoundClipOneShot(AudioClip clip, float volumeScale = 1.0f)
    {
        // Check the audio source component is valid
        if (audioSource)
        {
            // Check the clip is valid
            if (clip)
            {
                // Play the clip one shot
                audioSource.PlayOneShot(clip, volumeScale);
            }
            else
            {
                Debug.Log("PlaySoundClipOneShot attempting to play invalid audio clip");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the audio source component
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
