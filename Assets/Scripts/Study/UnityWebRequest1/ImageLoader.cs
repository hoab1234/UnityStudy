using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;

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

    private void ImageLoad(string keyword) => StartCoroutine(GetImageResources(keyword));

    IEnumerator GetImageResources(string keyword)
    {
        string url = baseUrl + keyword + "&tbm=isch";
        //string url = "https://www.google.com/search?q=" + keyword + "&source=lnms&tbm=isch";

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;
            
            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(UnityWebRequest.EscapeURL("https://search.naver.com/search.naver?sm=tab_hty.top&where=image&query=%EA%B0%9C&oquery=%EA%B0%9C.png&tqi=h8MMJsp0YidssuBlOo8ssssss3h-193639#imgId=image_sas%3Ablog157277402%7C6%7C222950714377_340112946"));
            yield return imageRequest.SendWebRequest();

            //Texture2D imageTexture = ((DownloadHandlerTexture)imageRequest.downloadHandler).texture;
            SetTexture(DownloadHandlerTexture.GetContent(www));
            // Assign the texture to the RawImage component
        } 
    }

    private void SetTexture(Texture2D texture)
    {
        imageList[0].texture = texture;
    }
}
