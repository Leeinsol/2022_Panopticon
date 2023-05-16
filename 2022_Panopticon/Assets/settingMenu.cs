using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class settingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider BgmSlider;
    public Slider SfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        float value = 0f;
        if (audioMixer.GetFloat("BGM", out value))
        {
            BgmSlider.value = Mathf.Pow(10f, value / 20f);
        }
        if (audioMixer.GetFloat("SFX", out value))
        {
            SfxSlider.value = Mathf.Pow(10f, value / 20f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
