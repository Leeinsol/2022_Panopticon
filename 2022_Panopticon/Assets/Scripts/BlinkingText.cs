using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    public GameObject BlinkText;
    public GameObject BlinkBackground;

    public static BlinkingText instance;
    public int count = 0;

    public bool hideWarning = false;

    private void Awake()
    {
        if (BlinkingText.instance == null)
        {
            BlinkingText.instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        BlinkText.SetActive(false);
        BlinkBackground.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        //if(count == 4)
        //{
        //    BlinkBackground.SetActive(false);
        //}
        if(BlinkText != null)
        {
            if (BlinkText.activeSelf && count == 0)
            {
                StartCoroutine(ShowText());
            }
        }
        
    }

    IEnumerator ShowText()
    {
        BlinkBackground.SetActive(true);

        //int count = 0;
        while (count < 3)
        {
            count++;

            //Debug.Log(count+"실행");

            BlinkText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            BlinkText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        if(count == 3)
        {
            BlinkText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Destroy(BlinkText);
            Destroy(BlinkBackground);
            hideWarning = true;
        }
    }
}
