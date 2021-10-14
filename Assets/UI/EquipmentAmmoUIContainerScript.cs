using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentAmmoUIContainerScript : MonoBehaviour
{
    [SerializeField] private GameObject uiImagePrefab;
    [SerializeField] private Vector2 ammoImageDimensions;
    [SerializeField] private float imagePadding = 2.0f;
    [SerializeField] private int initialAmmoImageCount = 3;

    private List<GameObject> ammoImages = new List<GameObject>();

    private void CreateAmmoImage(RectTransform containerRectTransform)
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
        imageRectTransform.sizeDelta = ammoImageDimensions;

        // Set the position of the image to the far left position of the container
        imageRectTransform.anchoredPosition = new Vector2(
            (-containerRectTransform.sizeDelta.x / 2.0f) + (imageRectTransform.sizeDelta.x / 2.0f) + (imageRectTransform.sizeDelta.x + imagePadding) * ammoImages.Count,
            0.0f);

        // Add the image to the list of ammo images
        ammoImages.Add(image);
    }

    public void RemoveAmmoImage()
    {
        // Check if there is at least 1 ammo image remaining
        if(ammoImages.Count > 0)
        {
            // Get the last image in the list
            var image = ammoImages[ammoImages.Count - 1];

            // Destroy the retrieved image
            Destroy(image);

            // Remove the image from the images list
            ammoImages.RemoveAt(ammoImages.Count - 1);
        }
    }

    public void AddAmmoImage()
    {
        // Create an ammo image
        CreateAmmoImage(gameObject.GetComponent<RectTransform>() as RectTransform);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Add ammo image up to the entered initial count
        for(int i = 0; i < initialAmmoImageCount; ++i)
        {
            AddAmmoImage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
