using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    public GameObject BlinkText;
    public static BlinkingText instance;
    public int count = 0;

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
        StartCoroutine(ShowText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ShowText()
    {
        //int count = 0;
        while (count < 3)
        {
            Debug.Log("실행");

            BlinkText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            BlinkText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            count++;
        }
    }
}
