namespace Quiz.Request
{
    using System;
    
    [Serializable]
    public class CreateResponseRequest
    {
        public long userId { get; set; }
        public long quizId { get; set; }
        public long questionId { get; set; }
        public long optionId { get; set; }
    }
}