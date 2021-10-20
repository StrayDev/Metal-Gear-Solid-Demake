using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GrowableImageContainer inventoryGrowableImageContainer = null;

    [SerializeField] private Sprite keycardTexture = null;

    public void OnKeycardPickedUp(KeycardScript keycard)
    {
        // Check the inventory image container has been set
        if(inventoryGrowableImageContainer)
        {
            // Add an image to the inventory container
            GameObject imageObject = inventoryGrowableImageContainer.AddImage();

            // Get the image component from the image
            var image = imageObject.GetComponent<Image>();

            // Check a keycard texture has been set
            if(!keycardTexture)
            {
                Debug.LogError("A keycard texture has not been set on the InGameUI instance");
                return;
            }

            // Set the texture of the image to the neutral keycard texture
            image.sprite = keycardTexture;

            // Check the image is valid
            if (image)
            {
                // Set the image color to the color of the keycard
                switch(keycard.GetKeycardColor())
                {
                    case KeycardScript.KeycardColor.Blue: image.color = Color.blue; break;
                    case KeycardScript.KeycardColor.Orange: image.color = new Color(0.255f * 6.0f, 0.165f * 6.0f, 0.0f); break;
                }
            }
        }
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
