using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;

    public void SetProgressBarValue(int value)
    {
        slider.value = value;
    }

    public void SetProgressBarValue(float value)
    {
        slider.value = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the slider component
        slider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
