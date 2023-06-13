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
    public KeyCode SprintKey = KeyCode.LeftShift;
    public KeyCode CrouchKey = KeyCode.LeftControl;
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode ZoomKey = KeyCode.Mouse1;
    public KeyCode FireKey = KeyCode.Mouse0;
    public KeyCode ReloadKey = KeyCode.R;
    // Start is called before the first frame update
    void Start()
    {
        content = ScrollView.transform.GetChild(0).GetChild(0).transform;

        KeyButton[0].GetComponentInChildren<TextMeshProUGUI>().text = SprintKey.ToString();

        GenerateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateButtons()
    {
        KeyCode[] keyCodes = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));

        for (int i = 0; i < keyCodes.Length; i++)
        {
            KeyCode keyCode = keyCodes[i];
            Debug.Log(keyCode);
            GameObject buttonGO = Instantiate(buttonPrefab, content);
            Button button = buttonGO.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
            Debug.Log(buttonText);
            buttonText.text = keyCode.ToString();
            
            button.onClick.AddListener(() => OnButtonClick(buttonText,keyCode));
        }
    }

    private void OnButtonClick(TextMeshProUGUI buttonText, KeyCode keyCode)
    {
        Debug.Log("Button clicked for KeyCode: " + keyCode);

        buttonText.text = keyCode.ToString();


    }
}
