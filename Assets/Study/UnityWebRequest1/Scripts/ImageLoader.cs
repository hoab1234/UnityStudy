using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private int maxImageCount = 10;
    [SerializeField] private ImageGenerator imageGenerator;
    private Texture[] textures;

    // 생성/다운로드 이미지 개수 맞춰야함

    private const string BASE_URL = "https://search.naver.com/search.naver?where=image&sm=tab_jum&query=";

    private void Awake()
    {
        //searchInputField, imageGenerator null check

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