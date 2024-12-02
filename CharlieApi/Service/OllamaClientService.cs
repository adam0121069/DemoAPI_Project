using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OllamaSharp;  // 用于解析 JSON 响应

namespace CharlieApi.Service
{
    public class QuestionAnswerService2
    {
        private readonly Uri _ollamaUri;
        private readonly string _modelName;
        public QuestionAnswerService2()
        {
            _ollamaUri = new Uri("http://localhost:11434");
            _modelName = "llama3.1";
        }

        public async Task<string> GetAnswerAsync(string prompt)
        {
            var ollama = new OllamaApiClient(_ollamaUri)
            {
                SelectedModel = _modelName
            };;
            var chat = new Chat(ollama);
           // 获取对话的响应流
            var responseStream = chat.SendAsync(prompt);
            string fullResponse = "";

            // 遍历异步流，拼接完整响应
            await foreach (var response in responseStream)
            {
                fullResponse += response;
            }

            return fullResponse; // 返回完整的响应
        }
    }
}


