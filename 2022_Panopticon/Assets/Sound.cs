using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Sound : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider BgmSlider;
    public Slider SfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (BgmSlider == null || SfxSlider == null)
        //{

        //    BgmSlider = GameObject.Find("Canvas").transform.Find("SettingPanel").GetChild(0).Find("BgmSlider").GetComponent<Slider>();
        //    SfxSlider = GameObject.Find("Canvas").transform.Find("SettingPanel").GetChild(0).Find("SfxSlider").GetComponent<Slider>();
        //}
    }

    public void SetBgmVolume()
    {
        Debug.Log(BgmSlider.value);
        audioMixer.SetFloat("BGM", Mathf.Log10(BgmSlider.value) * 20);
        //audioMixer.SetFloat("BGM", BgmSlider.value);
        
    }
    public void SetSfxVolume()
    {
        Debug.Log(SfxSlider.value);
        audioMixer.SetFloat("SFX", Mathf.Log10(SfxSlider.value) * 20);
        //audioMixer.SetFloat("SFX", SfxSlider.value);
    }
}
