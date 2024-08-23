using Microsoft.AspNetCore.Mvc;
using CharlieApi.Service;
using CharlieApi.Models;

namespace CharlieApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionAnswerController : ControllerBase
    {
        private readonly QuestionAnswerService _qaService;

        public QuestionAnswerController(QuestionAnswerService qaService)
        {
            _qaService = qaService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion([FromBody] QuestionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest("Question cannot be empty.");
            }

            var answer = await _qaService.GetAnswerAsync(request.Question);
            return Ok(new { question = request.Question, answer });
        }
    }
}