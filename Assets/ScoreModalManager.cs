using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreModalManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private QuizAPIOutbond quizAPIOutbond;

    [SerializeField]
    private TMP_Text scoreText;

    void Start()
    {
        quizAPIOutbond.OnScoreDataLoaded += ScoreDataLoadedEventHandler;
    }

    void OnEnable()
    {
        scoreText.text = "Caculating Score";
    }

    void OnDestroy()
    {
        quizAPIOutbond.OnScoreDataLoaded -= ScoreDataLoadedEventHandler;
    }

    void ScoreDataLoadedEventHandler(long score)
    {
        scoreText.text = $"{score}/{quizAPIOutbond.quizData.questions.Length}";
    }
}
