using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalImageContainer : MonoBehaviour
{
    [SerializeField] private GameObject uiImagePrefab;
    [SerializeField] private Vector2 imageDimensions = new Vector2(8.0f, 8.0f);
    [SerializeField] private float imagePadding = 2.0f;
    [SerializeField] private int initialImageCount = 3;
    // Controls the direction the image line grows along. Use -1 to start from the left and grow right and use 1 to start from the right and grow to the left.
    // Must be -1 or 1
    [SerializeField] private int horizontalAlignment = -1;

    private List<GameObject> images = new List<GameObject>();

    private void CreateImage(RectTransform containerRectTransform)
    {
        // Check a ui image prefab has been set in the editor
        if (!uiImagePrefab)
        {
            // Log error
            Debug.LogError("An UI image prefab has not been set");
            return;
        }

        // Spawn a new ui image prefab instance parented to the container
        GameObject image = Instantiate(uiImagePrefab, gameObject.transform);

        // Get the images rect transform component
        var imageRectTransform = image.GetComponent<RectTransform>() as RectTransform;

        // Check the image's rect transform is valid
        if (!imageRectTransform)
        {
            Debug.LogError("The set ui image prefab does not contain a rect transform component");
            return;
        }

        // Set the width and height of the image rect transform
        imageRectTransform.sizeDelta = imageDimensions;

        // Set the position of the image
        imageRectTransform.anchoredPosition = new Vector2(
            (horizontalAlignment * (containerRectTransform.sizeDelta.x / 2.0f)) + 
            (-horizontalAlignment * (imageRectTransform.sizeDelta.x / 2.0f)) + 
            (-horizontalAlignment * (imageRectTransform.sizeDelta.x + imagePadding))
            * images.Count,
            0.0f);

        // Add the image to the list of ammo images
        images.Add(image);
    }

    public void RemoveImage()
    {
        // Check if there is at least 1 image remaining
        if(images.Count > 0)
        {
            // Get the last image in the list
            var image = images[images.Count - 1];

            // Destroy the retrieved image
            Destroy(image);

            // Remove the image from the images list
            images.RemoveAt(images.Count - 1);
        }
    }

    public void AddImage()
    {
        // Create an ammo image
        CreateImage(gameObject.GetComponent<RectTransform>() as RectTransform);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add image up to the entered initial count
        for(int i = 0; i < initialImageCount; ++i)
        {
            AddImage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
