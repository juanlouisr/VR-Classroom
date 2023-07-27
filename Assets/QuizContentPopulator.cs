using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quiz.Request;
using Quiz.Response;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QuizContentPopulator : MonoBehaviour
{

    [SerializeField]
    private GameObject header;

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private GameObject buttonGroup;

    [SerializeField]
    private TMP_Text quizTitleField;

    private QuizAPIOutbond quizAPIOutbond;

    private Button[] buttons;

    private ToggleGroup toggleGroup;

    public int questionIdx = 0;

    void Start()
    {
        quizAPIOutbond = GetComponent<QuizAPIOutbond>();
        buttons = content.GetComponentsInChildren<Button>();
        toggleGroup = content.GetComponent<ToggleGroup>();
        buttons[0].onClick.AddListener(HandlePrv);
        buttons[1].onClick.AddListener(HandleNext);
        quizAPIOutbond.OnQuizDataLoaded += PopulateData;
    }

    void OnDestroy()
    {
        // Don't forget to unsubscribe from the event when this script is destroyed
        if (quizAPIOutbond != null)
        {
            quizAPIOutbond.OnQuizDataLoaded -= PopulateData;
        }
    }


    void PopulateData(object sender, EventArgs e)
    {
        quizTitleField.text = quizAPIOutbond.quizData.quizName;
        PopulateQuestions(quizAPIOutbond.quizData, 0);
    }

    void PopulateQuestions(QuizDto quizDto, int questionIdx)
    {
        buttons[0].transform.gameObject.SetActive(false);
        buttons[1].transform.gameObject.SetActive(false);

        if (questionIdx < 0 || questionIdx >= quizDto.questions.Length)
        {
            return;
        }

        header.GetComponent<TMP_Text>().text = quizDto.questions[questionIdx].questionText;

        if (questionIdx == quizDto.questions.Length - 1)
        {
            buttons[1].GetComponentInChildren<TMP_Text>().text = "Submit";
        }
        else
        {
            buttons[1].GetComponentInChildren<TMP_Text>().text = "Next";
        }

        PopulateOptions(quizDto, questionIdx);

        if (questionIdx > 0)
        {
            buttons[0].transform.gameObject.SetActive(true);
        }
        buttons[1].transform.gameObject.SetActive(true);
        
    }

    private void PopulateOptions(QuizDto quizDto, int questionIdx)
    {
        GameObject optionTemplate = content.transform.GetChild(0).gameObject;
        GameObject g;

        List<GameObject> options = new List<GameObject>();
        foreach (Toggle toggle in content.GetComponentsInChildren<Toggle>())
        {
            GameObject toggleParent = toggle.transform.parent.gameObject;
            options.Add(toggleParent);
        }

        foreach (var option in quizDto.questions[questionIdx].options)
        {
            g = Instantiate(optionTemplate, content.transform);
            g.transform.GetChild(0).GetComponent<TMP_Text>().text = option.optionText;
            g.GetComponent<OptionIndex>().data = option.id;

            var questionId = quizDto.questions[questionIdx].id;
            if (quizAPIOutbond.savedAnswer.TryGetValue(questionId, out long savedAnswer) && savedAnswer == option.id) 
            {
                g.GetComponentInChildren<Toggle>().isOn = true;
            }
        }

        buttonGroup.transform.SetAsLastSibling();

        foreach (var option in options)
        {
            Destroy(option);
        }
    }

    private void HandleNext()
    {
        
        var toggles = content.GetComponentsInChildren<Toggle>().Where(toggle => toggle.isOn);
        
        foreach (var toggle in toggles)
        {
            var questionid = quizAPIOutbond.quizData.questions[questionIdx].id;
            var optionId = toggle.transform.GetComponentInParent<OptionIndex>().data;
            quizAPIOutbond.savedAnswer[questionid] = optionId;
            StartCoroutine(quizAPIOutbond.SubmitQuestionOption(1, questionid, optionId));
        } 

        if (questionIdx < quizAPIOutbond.quizData.questions.Length-1)
        {
            PopulateQuestions(quizAPIOutbond.quizData, ++questionIdx);
        }
        
    
    }

    private void HandlePrv()
    {
        PopulateQuestions(quizAPIOutbond.quizData, --questionIdx);
    }



}
