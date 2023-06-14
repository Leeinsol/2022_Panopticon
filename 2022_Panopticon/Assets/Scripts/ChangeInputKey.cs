using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeInputKey : MonoBehaviour
{
    public GameObject ScrollView;
    public GameObject buttonPrefab;
    Transform content;
    public GameObject[] KeyButton;
    public KeyCode[] Keys;

    int currentIndex = 0;
    KeyCode[] keyCodes;
    private static ChangeInputKey instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static ChangeInputKey Instance
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
        PlayerPrefs.SetInt("Stage", 3);
        Keys[0]= KeyCode.LeftShift;
        Keys[1]= KeyCode.LeftControl;
        Keys[2]= KeyCode.Space;
        Keys[3]= KeyCode.Mouse1;
        Keys[4]= KeyCode.Mouse0;
        Keys[5]= KeyCode.R;


        keyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
        content = ScrollView.transform.GetChild(0).GetChild(0).transform;
        for(int i=0; i<KeyButton.Length; i++)
        {
            KeyButton[i].GetComponentInChildren<TextMeshProUGUI>().text = Keys[i].ToString();
        }

        GenerateButtons();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateButtons()
    {
         

        for (int i = 0; i < keyCodes.Length; i++)
        {
            KeyCode keyCode = keyCodes[i];
            GameObject buttonGO = Instantiate(buttonPrefab, content);
            Button button = buttonGO.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = keyCode.ToString();
            
            button.onClick.AddListener(() => OnButtonClick(buttonText,keyCode));
        }
    }

    private void OnButtonClick(TextMeshProUGUI buttonText, KeyCode keyCode)
    {
        Debug.Log("Button clicked for KeyCode: " + keyCode);

        KeyButton[currentIndex].GetComponentInChildren<TextMeshProUGUI>().text = keyCode.ToString();

        Keys[currentIndex] = keyCode;
        buttonState(keyCode);
    }

    public void setCurrentIndex(int index)
    {
        currentIndex = index;
        buttonState(Keys[currentIndex]);

        ScrollView.SetActive(true);
    }

    void buttonState(KeyCode keyCode)
    {
        int index = System.Array.IndexOf(keyCodes, keyCode);

        for (int i = 0; i < keyCodes.Length; i++)
        {
            content.GetChild(i).GetComponent<Button>().interactable = true;
            if(i==index)
                content.GetChild(i).GetComponent<Button>().interactable = false;

        }
        ScrollView.transform.GetChild(1).GetComponent<Scrollbar>().value = 1;

    }
}
