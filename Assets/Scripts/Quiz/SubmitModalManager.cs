using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubmitModalManager : MonoBehaviour
{
    [SerializeField]
    private GameObject quizSpatialPanel;

    [SerializeField]
    private GameObject quizScorePanel;

    [SerializeField]
    private QuizAPIOutbond quizAPIOutbond;

    [SerializeField]
    private Button cancelButton;
    
    [SerializeField]
    private Button confirmButton;

    void Start()
    {
        cancelButton.onClick.AddListener(HandleCancel);
        confirmButton.onClick.AddListener(HandleConfirm);
    }

    private void HandleCancel()
    {
        gameObject.SetActive(false);
    }

    private void HandleConfirm()
    {
        quizAPIOutbond.FinalizeQuiz();
        quizAPIOutbond.GetScore();
        gameObject.SetActive(false);
        quizScorePanel.SetActive(true);
        // quizSpatialPanel.SetActive(false);
    }

}
