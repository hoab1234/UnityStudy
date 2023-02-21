using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.IO;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private List<RawImage> imageList;
    [SerializeField] private int maxImageCount = 5;

    Image image;
    private string baseUrl = "https://search.naver.com/search.naver?where=image&sm=tab_jum&query=";

    private void Awake()
    {
        searchInputField.onSubmit.AddListener(ImageLoad);
    }

    private void ImageLoad(string keyword)
    {
        //StartCoroutine(GetRequest(baseUrl + searchInputField.text));
        StartCoroutine(GetRequest("https://search.naver.com/search.naver?where=image&sm=tab_jum&query=dog"));
    }

    IEnumerator GetRequest(string url)
    {
        string test = "https%3A%2F%2Fsearch.pstatic.net%2Fsunny%2F%3Fsrc%3Dhttps%253A%252F%252Fi.pinimg.com%252F736x%252Fbe%252F9a%252F53%252Fbe9a53cff7c5ef838562f5811f343ee5.jpg%26type%3Dsc960_832";
        Debug.Log(UnityWebRequest.UnEscapeURL(test));
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError) Debug.Log("error");
        else
        {
            Debug.Log(request.downloadHandler.text);
            string result = UnityWebRequest.UnEscapeURL(request.downloadHandler.text);
            //string compareString = "https://search.pstatic.net/sunny";
        }
    }
}
