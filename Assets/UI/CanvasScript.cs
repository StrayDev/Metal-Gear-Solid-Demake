using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro;
//using System; // Needed if using TMPro


public class CanvasScript : MonoBehaviour
{
    [SerializeField] private GameObject EquipmentAmmoContainer;
    [SerializeField] private GameObject TranqAmmoImagePrefab;

    private List<GameObject> EquipmentAmmoImages = new List<GameObject>();

    private void InitEquipmentAmmoImages(int count, GameObject ammoImagePrefab)
    {
        // Clear existing ammo images
        EquipmentAmmoImages.Clear();

        // Check the ammo image is valid
        if (ammoImagePrefab)
        {
            for(int i = 0; i < count; ++i)
            {
                // Create a new image
                GameObject image = Instantiate(TranqAmmoImagePrefab, gameObject.transform, false);

                // Get the rect transform from the image prefab
                RectTransform rectTransform = image.GetComponent<RectTransform>();

                // Check the rect transform is valid. Every image prefab must have a rect transform
                if(rectTransform)
                {
                    // Offset the image to fit in a horizontal line
                    rectTransform.anchoredPosition = new Vector2(i * (rectTransform.sizeDelta.x + 5), 0); // TODO: Replace 5 with padding variable

                    // Add the new image to the list of equipment ammo images
                    EquipmentAmmoImages.Add(image);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitEquipmentAmmoImages(3, TranqAmmoImagePrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
