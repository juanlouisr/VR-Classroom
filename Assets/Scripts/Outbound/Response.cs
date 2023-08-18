namespace Quiz.Response
{
    using System;
    using System.Collections.Generic;
    
    [Serializable]
    public class BaseResponse<T>
    {
        public bool success;
        public string message;
        public string error;
        public T data;
    }

    [Serializable]
    public class QuizDto
    {
        public long id;
        public string quizName;
        public string description;
        public DateTime createdAt;
        public QuestionDto[] questions;
    }

    [Serializable]
    public class QuestionDto
    {
        public long id;
        public long quizId;
        public string questionText;
        public OptionResponse[] options;
    }

    [Serializable]
    public class OptionResponse
    {
        public long id;
        public long questionId;
        public string optionText;
    }

    [Serializable]
    public class Response
    {
        public long userId;
        public long quizId;
        public long questionId;
        public long optionId;
        public DateTime responseTime;
        public ResponseStatus status = ResponseStatus.SAVED;
    }

    [Serializable]
    public class QuizIdentityResponse
    {
        public long userId;
        public long quizId;
    }

    public enum ResponseStatus
    {
        SAVED,
        FINAL
    }
}