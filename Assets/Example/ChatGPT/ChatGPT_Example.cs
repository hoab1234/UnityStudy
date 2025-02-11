using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;

public class ChatGPT_Example : MonoBehaviour
{
    [Serializable]
    private class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    private class ChatRequest
    {
        public string model;
        public Message[] messages;
    }

    [Serializable]
    private class Choice
    {
        public Message message;
    }

    [Serializable]
    private class ChatResponse
    {
        public Choice[] choices;
    }

    private const string OPENAI_API_ENDPOINT = "https://api.openai.com/v1/chat/completions";
    public string API_KEY = "your-api-key-here"; // API 키를 여기에 입력하세요
    public TMP_InputField inputField;
    public TMP_Text outputText;

    private void Start()
    {
        inputField.onSubmit.AddListener(async (text) => {
            string response = await GetChatCompletion(text);
            outputText.text = response;
            inputField.text = "";
        });
    }
 
    public async Task<string> GetChatCompletion(string userMessage)
    {
        var messages = new Message[]
        {
            new Message { role = "system", content = "You are a helpful assistant." },
            new Message { role = "user", content = userMessage }
        };

        var request = new ChatRequest
        {
            model = "gpt-4",
            messages = messages
        };

        string jsonRequest = JsonConvert.SerializeObject(request);

        using (UnityWebRequest webRequest = new UnityWebRequest(OPENAI_API_ENDPOINT, "POST"))
        {
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonRequest);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", $"Bearer {API_KEY}");

            try
            {
                var operation = webRequest.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    var response = JsonConvert.DeserializeObject<ChatResponse>(webRequest.downloadHandler.text);
                    return response.choices[0].message.content;
                }
                else
                {
                    Debug.LogError($"Error: {webRequest.error}");
                    return $"Error: {webRequest.error}";
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception: {e.Message}");
                return $"Exception: {e.Message}";
            }
        }
    }

    // 사용 예시
    private async void Example()
    {
        string response = await GetChatCompletion("Write a haiku about recursion in programming.");
        Debug.Log(response);
    }
}
