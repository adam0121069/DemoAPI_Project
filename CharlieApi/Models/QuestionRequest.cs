namespace CharlieApi.Models;

    public class QuestionRequest
    {
        public string Question { get; set; }

        public QuestionRequest()
        {
            Question = string.Empty;
        }
    }