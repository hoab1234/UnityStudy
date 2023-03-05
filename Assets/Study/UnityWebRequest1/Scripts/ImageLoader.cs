using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.IO;
using System;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private int maxImageCount = 10;
    [SerializeField] private ImageGenerator imageGenerator;

    // 생성/다운로드 이미지 개수 맞춰야함

    private string baseUrl = "https://search.naver.com/search.naver?where=image&sm=tab_jum&query=";

    private void Awake()
    {
        searchInputField.onSubmit.AddListener(value =>
        {
            ResetPreviousResult();
            RequestImage(baseUrl + value);
        });
    }

    private void RequestImage(string keyword)
    {
        StartCoroutine(GetRequest(keyword));
    }

    IEnumerator GetRequest(string url)
    {

        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError) Debug.Log("error");
        else
        {
            Debug.Log(request.downloadHandler.text);
            List<string> textureUrls = ExtractImageUrl(request.downloadHandler.text);
            StartCoroutine(DownloadImage(textureUrls));
        }
    }

    private List<string> ExtractImageUrl(string result)
    {
        List<string> textureUrls = new List<string>();

        string excludeKeyword = "\"originalUrl\":\"";
        int excludeIndex = excludeKeyword.Length;

        int imageCount = maxImageCount;

        while (imageCount != 0)
        {
            string data = result.Substring(result.IndexOf(excludeKeyword) + excludeIndex);

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
        Texture[] textures = new Texture[textureUrls.Count];

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
                Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
                textures[i] = texture;
            }
        }

        imageGenerator.Init(textures);
    }

    private void ResetPreviousResult()
    {
        if (imageGenerator != null)
        {

            if (imageGenerator.Objects != null)
            {
                foreach (var obj in imageGenerator.Objects)
                {
                    Destroy(obj);
                }
            }

            if (imageGenerator.Textures != null)
            {
                imageGenerator.Textures = null;
            }
        }
    }
}