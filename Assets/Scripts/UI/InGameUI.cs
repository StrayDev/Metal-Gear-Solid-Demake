using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GrowableImageContainer inventoryGrowableImageContainer = null;

    public void OnKeycardPickedUp(KeycardScript keycard)
    {
        // Check the inventory image container has been set
        if(inventoryGrowableImageContainer)
        {
            // Add an image to the inventory container
            GameObject imageObject = inventoryGrowableImageContainer.AddImage();

            // Get the image component from the image
            var image = imageObject.GetComponent<Image>();

            // Check the image is valid
            if (image)
            {
                // Set the image to the color of the keycard
                switch(keycard.GetKeycardColor())
                {
                    case KeycardScript.KeycardColor.Blue: image.color = Color.blue; break;
                    case KeycardScript.KeycardColor.Orange: image.color = new Color(255, 165, 0); break;
                }
            }
        }

        Debug.Log("Picked up a keycard" + keycard.GetKeycardColor());
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
