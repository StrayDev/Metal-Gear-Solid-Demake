using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GrowableImageContainer : MonoBehaviour
{
    enum EDirection
    {
        Vertical = 0,
        Horizontal,
    };

    [SerializeField] private GameObject uiImagePrefab;
    [SerializeField] private Vector2 imageDimensions = new Vector2(8.0f, 8.0f);
    [SerializeField] private float imagePadding = 2.0f;
    [SerializeField] private int initialImageCount = 3;
    // Controls the direction the image line will grow in
    [SerializeField] private EDirection direction = EDirection.Horizontal;
    // Controls the direction the image line starts from and grows along. Use -1 to start from the
    // left/bottom and grow to the right/top and use 1 to start from the right/top and grow to the left/bottom. Must be -1 or 1
    [SerializeField] private int alignment = -1;

    private List<GameObject> images = new List<GameObject>();

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

    public GameObject AddImage()
    {
        // Create an image
        return CreateImage(gameObject.GetComponent<RectTransform>());
    }

    public Image GetImageAt(int index)
    {
        return images[index].GetComponent<Image>();
    }

    public void SetAllImagesTo(Sprite sprite)
    {
        for(int i = 0; i < images.Count; ++i)
        {
            images[i].GetComponent<Image>().sprite = sprite;
        }
    }

    public void SetImageTo(Sprite sprite, int imageIndex)
    {
        images[imageIndex].GetComponent<Image>().sprite = sprite;
    }

    public int GetImageCount()
    {
        return images.Count;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        // Add image up to the entered initial count
        for(int i = 0; i < initialImageCount; ++i)
        {
            AddImage();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private GameObject CreateImage(RectTransform containerRectTransform)
    {
        // Check a ui image prefab has been set in the editor
        if (!uiImagePrefab)
        {
            // Log error
            Debug.LogError("An UI image prefab has not been set");
            return null;
        }

        // Spawn a new ui image prefab instance parented to the container
        GameObject image = Instantiate(uiImagePrefab, gameObject.transform);

        // Get the images rect transform component
        var imageRectTransform = image.GetComponent<RectTransform>() as RectTransform;

        // Check the image's rect transform is valid
        if (!imageRectTransform)
        {
            Debug.LogError("The set ui image prefab does not contain a rect transform component");
            return null;
        }

        // Set the width and height of the image rect transform
        imageRectTransform.sizeDelta = imageDimensions;

        // Switch on the grow direction to correctly position the new image
        switch (direction)
        {
            case EDirection.Horizontal:
                // Position the image horizontally
                imageRectTransform.anchoredPosition = new Vector2(
                    (alignment * (containerRectTransform.sizeDelta.x / 2.0f)) +
                    (-alignment * (imageRectTransform.sizeDelta.x / 2.0f)) +
                    (-alignment * (imageRectTransform.sizeDelta.x + imagePadding))
                    * images.Count,
                    0.0f);
                break;

            case EDirection.Vertical:
                // Position the image vertically
                imageRectTransform.anchoredPosition = new Vector2(
                    0.0f,
                    (alignment * (containerRectTransform.sizeDelta.y / 2.0f)) +
                    (-alignment * (imageRectTransform.sizeDelta.y / 2.0f)) +
                    (-alignment * (imageRectTransform.sizeDelta.y + imagePadding))
                    * images.Count);
                break;
        }

        // Add the image to the list of ammo images
        images.Add(image);

        // Return the new image
        return image;
    }
}
