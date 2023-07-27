using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Quiz.Response;
using Quiz.Request;
using System;

public class QuizAPIOutbond : MonoBehaviour
{
    private const string apiUrl = "https://vr-backend-production.up.railway.app/api/quiz/";

    public QuizDto quizData;

    public delegate void QuizDataLoadedEventHandler(object sender, EventArgs e);

    // Define the event using the delegate
    public event QuizDataLoadedEventHandler OnQuizDataLoaded;

    private void Start()
    {
        FetchQuiz(1);
    }

    public void FetchQuiz(long id) 
    {
        StartCoroutine(GetQuizData(id));
    }

    public IEnumerator GetQuizData(long quizId)
    {
        quizData = null;

        UnityWebRequest request = UnityWebRequest.Get(apiUrl + quizId);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResult = request.downloadHandler.text;

            Debug.Log(jsonResult);

            // Parse the JSON response using JsonUtility
            BaseResponse<QuizDto> responseData = JsonUtility.FromJson<BaseResponse<QuizDto>>(jsonResult);

            // Now you can access the quiz data from the response
            if (responseData.success)
            {
                quizData = responseData.data;
                Debug.Log("Quiz Name: " + quizData.quizName);
                QuizDataFetched(quizData, EventArgs.Empty);
            }
            else
            {
                Debug.LogError("API Error: " + responseData.error);
            }
        }
    }

    protected virtual void QuizDataFetched(QuizDto quizDto, EventArgs e)
    {
        OnQuizDataLoaded?.Invoke(quizData, e);
    }

    public IEnumerator SubmitQuestionOption(long userId, long questionId, long optionId)
    {
        CreateResponseRequest requestData = new CreateResponseRequest
        {
            userId = userId,
            quizId = quizData.id,
            questionId = questionId,
            optionId = optionId
        };

        string jsonData = JsonUtility.ToJson(requestData);
        UnityWebRequest request = new UnityWebRequest(apiUrl + "/answer", "POST");
        request.SetRequestHeader("Content-Type", "application/json");


        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(postData);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("POST request failed: " + request.error);
        }
        else
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
    }

}
