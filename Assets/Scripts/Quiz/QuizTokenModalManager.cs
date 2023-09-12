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
    private GameObject errorPanel;

    [SerializeField]
    private GameObject title;

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
        quizTokenOutbound.OnOutboundError += OutboundErrorEventHandler;
        button.onClick.AddListener(HandleDecrypt);
        inputField.onSubmit.AddListener(x => HandleDecrypt());
    }
    void OnDestroy()
    {
        quizTokenOutbound.OnTokenValid -= TokenValidEventHandler;
        quizTokenOutbound.OnOutboundError -= OutboundErrorEventHandler;
    }

    void OnEnable()
    {
        title.SetActive(true);
    }

    void OnDisable()
    {
        title.SetActive(false);
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

    void OutboundErrorEventHandler(string error)
    {
        errorPanel.GetComponentsInChildren<TMP_Text>()[1].text = error;
        gameObject.SetActive(false);
        errorPanel.SetActive(true);
    }

    
}
