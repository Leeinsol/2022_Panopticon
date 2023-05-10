using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class gameOver : MonoBehaviour
{
    public GameObject remainEnemyText;
    public Slider progressBar;
    float percent;
    float animationSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        remainEnemyText.GetComponent<Text>().text = PlayerPrefs.GetInt("remainEnemy").ToString();
        percent = 100 - (PlayerPrefs.GetInt("remainEnemy") / (float)PlayerPrefs.GetInt("AllEnemy")) * 100f;
        //percent = 80.1234f;
        Debug.Log(percent);

        StartCoroutine(AnimateSlider());
    }

    // Update is called once per frame
    void Update()
    {
        remainEnemyText.GetComponent<Text>().text = string.Format("{0:N1}", progressBar.value) + "%";
    }

    IEnumerator AnimateSlider()
    {
        float currentValue = progressBar.value;
        float targetValue = percent;

        while (currentValue < targetValue)
        {
            //currentValue += animationSpeed * Time.deltaTime;
            currentValue += animationSpeed;
            progressBar.value = currentValue;
            yield return null;
        }
        progressBar.value = targetValue;
        yield return new WaitForSeconds(0.3f);
    }
}
