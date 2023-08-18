using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinCodeManager : MonoBehaviour
{
    [SerializeField]
    public Button joinButton;

    [SerializeField]
    private TMP_InputField inputField;


    void Start()
    {
        joinButton.onClick.AddListener(JoinClient);
    }

    void JoinClient()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            SceneTransitionManager.singleton.InitiatlizeAsHost = false;
            SceneTransitionManager.singleton.JoinCode = inputField.text;
            SceneTransitionManager.singleton.GoToSceneAsync(1);
        }
    }
}
