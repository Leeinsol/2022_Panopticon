using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

[System.Serializable]
public class InputKey
{
    public InputKey(string _keyName, string _keyCode) {KeyName=_keyName; KeyCode = _keyCode; }
    public string KeyName,KeyCode;
}

public class ChangeInputKey : MonoBehaviour
{
    public GameObject ScrollView;
    public GameObject buttonPrefab;
    Transform content;
    public GameObject[] KeyButton;
    public KeyCode[] Keys;

    int currentIndex = 0;
    KeyCode[] keyCodes;

    public TextAsset InputKeyDatabase;
    public List<InputKey> keyList, myKeyList;
    string filePath;


    // Start is called before the first frame update
    void Start()
    {

        string[] line = InputKeyDatabase.text.Substring(0, InputKeyDatabase.text.Length - 1).Split('\n');
        for(int i=0; i<line.Length; ++i)
        {
            string[] row = line[i].Split('\t');
            keyList.Add(new InputKey(row[0], row[1]));
        }
        filePath = Application.persistentDataPath + "/MyKeyList.txt";

        Load();

        keyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
        content = ScrollView.transform.GetChild(0).GetChild(0).transform;
        for(int i=0; i<KeyButton.Length; i++)
        {
            KeyButton[i].GetComponentInChildren<TextMeshProUGUI>().text = myKeyList[i].KeyCode;
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

        SetInputKey(keyList[currentIndex].KeyName, keyCode);
        
        buttonState(keyCode);
    }

    public void setCurrentIndex(int index)
    {
        currentIndex = index;
        string key = myKeyList[currentIndex].KeyCode;
        buttonState((KeyCode)System.Enum.Parse(typeof(KeyCode),key));

        ScrollView.SetActive(true);
        setKeyButtonInteractable();
        KeyButton[currentIndex].GetComponent<Button>().interactable = false;

    }

    void setButtonName()
    {
        for(int i=0; i<KeyButton.Length; i++)
        {
            KeyButton[i].GetComponentInChildren<TextMeshProUGUI>().text = myKeyList[i].KeyCode.ToString();
        }

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

    void setKeyButtonInteractable()
    {
        for(int i =0; i<KeyButton.Length; ++i)
        {
            KeyButton[i].GetComponent<Button>().interactable = true;
        }
    }

    void SetInputKey(string keyName,KeyCode keyCode)
    {
        InputKey inputkey = myKeyList.Find(x => x.KeyName == keyName);
        if (inputkey != null)
        {
            inputkey.KeyCode = keyCode.ToString();
        }
        Save();
    }

    void Save()
    {
        string jdata = JsonUtility.ToJson(new Serialization<InputKey>(myKeyList));

        File.WriteAllText(filePath, jdata);
    }

    public void resetInputSetting()
    {
        defaultInputKey();

        Save();
        setKeyButtonInteractable();
        ScrollView.SetActive(false);
        ScrollView.transform.GetChild(1).GetComponent<Scrollbar>().value = 1;
        setButtonName();
    }
    void defaultInputKey()
    {
        myKeyList = new List<InputKey>();
        myKeyList.Add(new InputKey("Sprint", "LeftShift"));
        myKeyList.Add(new InputKey("Crouch", "LeftControl"));
        myKeyList.Add(new InputKey("Jump", "Space"));
        myKeyList.Add(new InputKey("Zoom", "Mouse1"));
        myKeyList.Add(new InputKey("Fire", "Mouse0"));
        myKeyList.Add(new InputKey("Reload", "R"));
    }

    void Load()
    {
        if (!File.Exists(filePath)) {
            defaultInputKey();
            return; 
        }

        string jdata = File.ReadAllText(filePath);
        myKeyList = JsonUtility.FromJson<Serialization<InputKey>>(jdata).target;
    }
}
