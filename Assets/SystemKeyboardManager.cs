using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SystemKeyboardManager : MonoBehaviour
{

    private TouchScreenKeyboard keyboard;

    private TMP_InputField inputField;


    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        inputField = GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener(initial => OpenSystemKeyboard(initial));
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboard != null)
        {
            inputField.text = keyboard.text;
            // Do stuff with keyboardText
        }
    }

    public void OpenSystemKeyboard(string initial)
    {
        keyboard = TouchScreenKeyboard.Open(initial, TouchScreenKeyboardType.Default);
    }

}
