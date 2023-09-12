using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizTokenModalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject quizPanel;

    [SerializeField]
    private Button button;

    private QuizTokenOutbound quizTokenOutbound;

    private TMP_InputField inputField;

    

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        quizTokenOutbound = GetComponent<QuizTokenOutbound>();
        quizTokenOutbound.OnTokenValid += TokenValidEventHandler;
        button.onClick.AddListener(HandleDecrypt);
        inputField.onSubmit.AddListener(x => HandleDecrypt());
    }
    void OnDestroy()
    {
        quizTokenOutbound.OnTokenValid -= TokenValidEventHandler;
    }

    private void HandleDecrypt()
    {
        if (inputField.text != "")
        {
            quizTokenOutbound.DecryptToken(inputField.text);
        }
    }

    void TokenValidEventHandler(long userId, long quizId)
    {
        var quizAPIOutbond = quizPanel.GetComponent<QuizAPIOutbond>();
        quizAPIOutbond.userId = userId;
        quizAPIOutbond.quizId = quizId;
        gameObject.SetActive(false);
        quizPanel.SetActive(true);
    }

    
}
