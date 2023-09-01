using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;


public class StageSetting : MonoBehaviour
{
    public AudioSource BGMSource;
    public AudioSource SfxSource;

    private static StageSetting instance = null;

    private void Awake()
    {
        getAudioSource();
        if (instance == null)
        {
            Debug.Log("instance==null");
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static StageSetting Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void getAudioSource()
    {
        BGMSource = GetComponent<AudioSource>();

    }
}
