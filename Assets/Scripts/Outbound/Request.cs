namespace Quiz.Request
{
    using System;
    
    [Serializable]
    public class CreateResponseRequest
    {
        public long userId;
        public long quizId;
        public long questionId;
        public long optionId;
    }

    [Serializable]
    public class FinalizeResponseRequest
    {
        public long userId;
        public long quizId;
    }
}