using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class ImageLoader : MonoBehaviour
{
    // 불필요한 함수, 변수, 지역변수, 매개변수 삭제하면서 코드량 줄이고 직관적으로 시도해볼 것
    // 네이밍이 목적에 맞고 명확하게 변경할 것
    // 해당 클래스가 가진 목적에 집중하고, 그 목적에 필요한 데이터를 밖에서 의존성 가지지 말고 가지고 있도록 할 것

    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private int maxImageCount = 10;
    [SerializeField] private ImageGenerator imageGenerator;
    private Texture[] textures;

    //변하면 안되는 값이기 때문에, 상수로 변경 및 네이밍도 더 명확하게
    private string BASE_URL = "https://search.naver.com/search.naver?where=image&sm=tab_jum&query=";

    private void Awake()
    {
        // Inputfield의 onSubmit 변수도 value보다 더 명확하게 -> keyword 처럼
        searchInputField.onSubmit.AddListener(keyword =>
        {
            ResetPreviousResult();
            StartCoroutine(GetRequest(BASE_URL + keyword));
        });
    }

    IEnumerator GetRequest(string urlWithKeyword)
    {
        UnityWebRequest request = UnityWebRequest.Get(urlWithKeyword);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError) Debug.Log("error");
        else
        {
            Debug.Log(request.downloadHandler.text);
            List<string> textureUrls = ExtractImageUrlList(request.downloadHandler.text);
            StartCoroutine(DownloadImage(textureUrls));
        }
    }

    /* 이해를 돕기 위해 변수명으로 잡아둔 것은 괜찮았다고 개인적으로 생각하지만
    정말 해당 방법이 괜찮은지, 더 직관적으로 바뀔 순 없는지 고민해봐야할 듯
    목적에 맞지 않았고 오히려 더 이해하기 어려워 졌었었음*/
    private List<string> ExtractImageUrlList(string result)
    {
        List<string> textureUrls = new List<string>();

        string excludeKeyword = "\"originalUrl\":\"";

        int imageCount = maxImageCount;

        while (imageCount != 0)
        {
            int keywordStartIndex = result.IndexOf(excludeKeyword) + excludeKeyword.Length;
            string data = result.Substring(keywordStartIndex);
            string url = data.Substring(0, data.IndexOf("\""));

            textureUrls.Add(UnityWebRequest.UnEscapeURL(url));
            Debug.Log(UnityWebRequest.UnEscapeURL(url));
            result = data.Substring(data.IndexOf("\"") + 1);
            imageCount--;
        }
        return textureUrls;
    }

    IEnumerator DownloadImage(List<string> textureUrls)
    {
        textures = new Texture[textureUrls.Count];

        for (int i = 0; i < textureUrls.Count; i++)
        {
            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(textureUrls[i]);

            yield return imageRequest.SendWebRequest();

            if (imageRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"Error downloading image: {imageRequest.error}");
            }
            else
            {
                textures[i] = DownloadHandlerTexture.GetContent(imageRequest);
            }
        }

        imageGenerator.Init(textures);
    }

    /*Destroy 하면 데이터 보관이 끝나는 것이 아님*/
    
    private void ResetPreviousResult()
    {
        if (textures != null)
        {
            foreach (var texture in textures)
            {
                Destroy(texture);
            }
            textures = null;
        }
    }
}