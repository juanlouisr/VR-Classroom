using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quiz.Request;
using Quiz.Response;
using UnityEngine;
using UnityEngine.UI;


public class QuizContentPopulator : MonoBehaviour
{

    [SerializeField]
    GameObject header;

    [SerializeField]
    GameObject content;

    QuizAPIOutbond quizAPIOutbond;

    Button[] buttons;

    ToggleGroup toggleGroup;

    private int questionIdx = 0;

    void Start()
    {
        quizAPIOutbond = GetComponent<QuizAPIOutbond>();
        buttons = content.GetComponentsInChildren<Button>();
        toggleGroup = content.GetComponent<ToggleGroup>();
        quizAPIOutbond.OnQuizDataLoaded += PopulateData;
        buttons[0].onClick.AddListener(HandlePrv);
        buttons[1].onClick.AddListener(HandleNext);

    }

    void OnDestroy()
    {
        // Don't forget to unsubscribe from the event when this script is destroyed
        if (quizAPIOutbond != null)
        {
            quizAPIOutbond.OnQuizDataLoaded -= PopulateData;
        }
    }


    void PopulateData(object quizData, EventArgs e)
    {
        Text headerText = header.GetComponent<Text>();  
        // Debug.Log("Populate dataa " +  (headerText.text == null ? "null":"exist"));
        headerText.text = quizAPIOutbond.quizData.quizName;
        GenerateContent(quizAPIOutbond.quizData, 0);
       
    }

    void GenerateContent(QuizDto quizDto, int questionIdx) 
    {
        Debug.Log("Generating content");
        buttons[0].transform.gameObject.SetActive(false);
        buttons[1].transform.gameObject.SetActive(false);

        if (questionIdx < 0)
        {
            return;
        }
        buttons[1].transform.gameObject.SetActive(true);
        if (questionIdx >= quizDto.questions.Length)
        {
            return;
        }
        if (questionIdx == quizAPIOutbond.quizData.questions.Length-1)
        {
            buttons[1].GetComponentInChildren<Text>().text = "Submit";
        }
        buttons[0].transform.gameObject.SetActive(true);


        GameObject optionTemplate = content.transform.GetChild(0).gameObject;
        GameObject g;

        List<GameObject> options = new List<GameObject>();
        for (int i = 0; i < content.transform.childCount-1; i++)
        {
            options.Add(content.transform.GetChild(i).gameObject);
        }

        foreach (var option in quizDto.questions[questionIdx].options)
        {
            g = Instantiate(optionTemplate, content.transform);
            g.transform.GetChild(0).GetComponent<Text>().text = option.optionText;
            OptionIndex optionIndex = g.AddComponent<OptionIndex>();
            optionIndex.data = option.id;
        }

        foreach (var option in options)
        {
            Destroy(option);
        }

        
         content.GetComponent<ToggleGroup>().SetAllTogglesOff();
    }

    private void HandleNext()
    {
        if (questionIdx < quizAPIOutbond.quizData.questions.Length-1)
        {
            GenerateContent(quizAPIOutbond.quizData, ++questionIdx);
        }


        var toggles = toggleGroup.ActiveToggles().Where(toggle => toggle.isOn);
        
        foreach (var toggle in toggles)
        {
            var optionId = toggle.transform.GetComponentInParent<OptionIndex>();
            StartCoroutine(quizAPIOutbond.SubmitQuestionOption(1, questionIdx, optionId.data));
        }        
        
    
    }

    private void HandlePrv()
    {
        GenerateContent(quizAPIOutbond.quizData, --questionIdx);
    }



}
