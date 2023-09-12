using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using TMPro;
using UnityEngine;

public class SystemKeyboardManager : MonoBehaviour
{

    private TMP_InputField inputField;

    private float distance = -0.5f;

    private float verticalOffset = -0.5f;

    private new Transform transform;


    // Start is called before the first frame update
     void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener(_ => OpenKeyboard());
        NonNativeKeyboard.Instance.OnClosed += UnlinkInputField;
        transform = inputField.transform;
    }

    private void OpenKeyboard()
    {
        NonNativeKeyboard.Instance.InputField = inputField;
        NonNativeKeyboard.Instance.PresentKeyboard(inputField.text);

        Vector3 direction = transform.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPosition = transform.position + direction * distance + Vector3.up * verticalOffset;
        NonNativeKeyboard.Instance.RepositionKeyboard(targetPosition);
    }

    private void UnlinkInputField(object sender, EventArgs e)
    {
        NonNativeKeyboard.Instance.InputField = null;
    }

    private void OnDestroy()
    {
        if (NonNativeKeyboard.Instance != null)
        {
            NonNativeKeyboard.Instance.OnClosed -= UnlinkInputField;
        }
    }

}
