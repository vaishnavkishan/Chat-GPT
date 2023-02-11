using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter a question:");
        var question = Console.ReadLine();

        var response = GetResponseFromOpenAI(question);
        Console.WriteLine("Answer: " + response);
    }

    private static string? GetResponseFromOpenAI(string? question)
    {
        var apiKey = "{put_api_key}";
        var prompt = $"{question}";

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.openai.com/v1/completions");
        request.Content = new StringContent(JsonConvert.SerializeObject(
            new OpenAI_Request
            {
                model = "text-davinci-003",
                prompt = "Computer programmer Joke about:"+prompt,
                temperature = 1,
                max_tokens = 128,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0
            }), Encoding.UTF8, "application/json");

        var response = httpClient.SendAsync(request).Result;
        response.EnsureSuccessStatusCode();

        var responseString = response.Content.ReadAsStringAsync().Result;
        var responseJson = JsonConvert.DeserializeObject<OpenAI_Response>(responseString);

        return responseJson?.choices?[0].text;
    }
}
class OpenAI_Request
{
    public string? model { get; set; }
    public string? prompt { get; set; }
    public double? temperature { get; set; }
    public int? max_tokens { get; set; }
    public double? top_p { get; set; }
    public double? frequency_penalty { get; set; }
    public double? presence_penalty { get; set; }
}

class Choice
{
    public string? text { get; set; }
}

class OpenAI_Response
{
    public Choice[]? choices { get; set; }
}