
namespace CharlieApi.Models;

public class AnswerResponse
{
    public string Question { get; set; }
    public string Answer { get; set; }

    public AnswerResponse()
    {
        Question = string.Empty;
        Answer =  string.Empty;
    }
}