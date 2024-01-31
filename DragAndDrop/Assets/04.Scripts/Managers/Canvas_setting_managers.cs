using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_setting_managers : MonoBehaviour
{
    public Slider bgm_slider;
    public Slider sfx_slider;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        bgm_slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnSliderValueChanged(float value)
    {
        // Do something with the changed value, for example, print it to the console
        Debug.Log("Slider Value: " + value);
    }
}
