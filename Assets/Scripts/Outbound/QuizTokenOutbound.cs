using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Quiz.Response;
using Quiz.Request;
using System;


public class QuizTokenOutbound : MonoBehaviour
{
    private static readonly string apiUrl = "https://vr-backend-production.up.railway.app/api/quiz";

    public delegate void QuizIdentityLoaded(long userId, long quizId);

    public event QuizIdentityLoaded OnTokenValid;

    public delegate void OutboundError(string error);

    public event OutboundError OnOutboundError;

    public void DecryptToken(string data)
    {
        Debug.Log("decrypting token " + data);
        StartCoroutine(IDecryptToken(data));
    }

    private IEnumerator IDecryptToken(string data)
    { 
        var decryptTokenUri = new UriBuilder(apiUrl + "/decode-token");
        decryptTokenUri.Query = $"data={data}";
        
        var questionCountRequest = UnityWebRequest.Get(decryptTokenUri.ToString());

        yield return questionCountRequest.SendWebRequest();

        if (questionCountRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + questionCountRequest.error);
            OnOutboundError?.Invoke(questionCountRequest.error);  
        }
        else
        {
            string jsonResult = questionCountRequest.downloadHandler.text;

            Debug.Log(jsonResult);
            BaseResponse<QuizIdentityResponse> responseData = JsonUtility.FromJson<BaseResponse<QuizIdentityResponse>>(jsonResult);

            if (responseData.success)
            {
                OnTokenValid?.Invoke(responseData.data.userId, responseData.data.quizId);
            }
            else
            {
                OnOutboundError?.Invoke(responseData.error);
                Debug.LogError("API Error: " + responseData.error);
            }
        }
    }
}
