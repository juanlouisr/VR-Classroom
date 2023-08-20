using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Quiz.Response;
using Quiz.Request;
using System;

public class QuizAPIOutbond : MonoBehaviour
{
    private static readonly string apiUrl = "https://vr-backend-production.up.railway.app/api/quiz";

    [SerializeField]
    public long userId = 1;

    [SerializeField]
    public long quizId = 1;

    public QuizDto quizData;

    public Dictionary<long, long> savedAnswer;

    public delegate void QuizDataLoadedEventHandler(object sender, EventArgs e);
    public delegate void ScoreDataLoadedEventHandler(long score);

    // Define the event using the delegate
    public event QuizDataLoadedEventHandler OnQuizDataLoaded;
    public event ScoreDataLoadedEventHandler OnScoreDataLoaded;



    private void OnEnable()
    {
        FetchQuiz();
    }

    private void OnDisable()
    {
        quizData = null;
        savedAnswer = null;
    }

    public void FetchQuiz() 
    {
        StartCoroutine(GetQuizData());
    }

    public void FetchScore() 
    {
        StartCoroutine(GetQuizScore());
    }

    public void SubmitQuestion(long questionId, long optionId) 
    {
        StartCoroutine(SubmitQuestionOption(questionId, optionId));
    }

    public void FinalizeQuiz() 
    {
        StartCoroutine(FinalizeQuizAnswer());
    }
    public void GetScore() 
    {
        StartCoroutine(GetQuizScore());
    }

    public IEnumerator GetQuizData()
    {
        quizData = null;
        savedAnswer = null;
        
        var uriBuilder = new UriBuilder(apiUrl + "/response");
        uriBuilder.Query = $"userId={userId}&quizId={quizId}";
        
        var quizRequest = UnityWebRequest.Get(apiUrl + "/" + quizId);
        var responseRequest = UnityWebRequest.Get(uriBuilder.ToString());

        var op1 = quizRequest.SendWebRequest();
        var op2 = responseRequest.SendWebRequest();


        yield return new WaitUntil(() => op1.isDone && op2.isDone);

        // load saved answer
        if (responseRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + responseRequest.error);
        }
        else
        {
            string jsonResult = responseRequest.downloadHandler.text;

            Debug.Log(jsonResult);
            BaseResponse<Response[]> responseData = JsonUtility.FromJson<BaseResponse<Response[]>>(jsonResult);
            
            if (responseData.success)
            {
                savedAnswer = new Dictionary<long, long>();
                foreach (var resp in responseData.data)
                {
                    savedAnswer[resp.questionId] = resp.optionId;
                }
            }
            else
            {
                Debug.LogError("API Error: " + responseData.error);
            }
        }

        // load quizdto and event invocation
        if (quizRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + quizRequest.error);
        }
        else
        {
            string jsonResult = quizRequest.downloadHandler.text;

            Debug.Log(jsonResult);

            // Parse the JSON response using JsonUtility
            BaseResponse<QuizDto> responseData = JsonUtility.FromJson<BaseResponse<QuizDto>>(jsonResult);

            // Now you can access the quiz data from the response
            if (responseData.success)
            {
                quizData = responseData.data;
                if (savedAnswer == null)
                {
                    savedAnswer = new Dictionary<long, long>();
                }
                Debug.Log("Quiz Name: " + quizData.quizName);
                QuizDataFetched(quizData, EventArgs.Empty);
            }
            else
            {
                Debug.LogError("API Error: " + responseData.error);
            }
        }
    }

    public IEnumerator GetQuizScore()
    {
        var scoreUri = new UriBuilder(apiUrl + "/score")
        {
            Query = $"userId={userId}&quizId={quizId}"
        };
        var scoreRequest = UnityWebRequest.Get(scoreUri.ToString());
        Debug.Log("getting score");
        var op1 = scoreRequest.SendWebRequest();
        yield return new WaitUntil(() => op1.isDone);
        
        if (scoreRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + scoreRequest.error);
        }
        else
        {
            string jsonResult = scoreRequest.downloadHandler.text;

            Debug.Log(jsonResult);
            BaseResponse<long> responseData = JsonUtility.FromJson<BaseResponse<long>>(jsonResult);

            if (responseData.success)
            {
                long score = responseData.data;
                Debug.Log("Success, score: " + score);
                OnScoreDataLoaded?.Invoke(score);
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

    private IEnumerator SubmitQuestionOption(long questionId, long optionId)
    {
        CreateResponseRequest requestData = new CreateResponseRequest
        {
            userId = userId,
            quizId = quizId,
            questionId = questionId,
            optionId = optionId
        };
        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log(jsonData);
        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl + "/response", jsonData, "application/json"))
        {
            yield return request.SendWebRequest();
            
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("POST request failed: " + request.error);
            }
            else
            {
                string jsonResult = request.downloadHandler.text;
                savedAnswer[questionId] = optionId;
                Debug.Log(jsonResult);
            }
        }
    }

    private IEnumerator FinalizeQuizAnswer()
    {
        FinalizeResponseRequest requestData = new FinalizeResponseRequest
        {
            userId = userId,
            quizId = quizId
        };
        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log(jsonData);
        using (UnityWebRequest request = UnityWebRequest.Post(apiUrl + "/response/finalize", jsonData, "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("POST request failed: " + request.error);
            }
            else
            {
                string jsonResult = request.downloadHandler.text;
                Debug.Log(jsonResult);
            }
        }
    }

}
