using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageGenerator : MonoBehaviour
{
    [SerializeField] private List<Transform> generatePointTransList;
    [SerializeField] private GameObject imagePrefab;

    [Space(20)]
    [Header("Customize Image Property")]
    [SerializeField] private int totalImageCount = 10;
    [SerializeField] private float imageQuadSpeed = 10f;
    [SerializeField] private float generateTermTime = 1f;
    [SerializeField] private int rotateTerm = 2;

    private WaitForSeconds waitForGenerate;
    private RawImage[] rawImages;

    private void Awake()
    {
        waitForGenerate = new WaitForSeconds(generateTermTime);
        DebugPageText();
    }

    void DebugPageText()
    {
        StartCoroutine(GetRequest("https://search.naver.com/search.naver?where=image&sm=tab_jum&query=dog"));
    }

    IEnumerator GetRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError) Debug.Log("error");
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    IEnumerator Start()
    {
        // get images from image
        //rawImages = GetImages();

        for (int i = 0; i < totalImageCount ; i++)
        {
            GameObject imageQuad = Instantiate(imagePrefab, generatePointTransList[Random.Range(0, generatePointTransList.Count)]);
            ImageObject imageObject = imageQuad.GetComponent<ImageObject>();

            //imageObject.SetImage(rawImages[i].mainTexture);
            imageObject.SetSpeed(imageQuadSpeed);

            yield return waitForGenerate;
        }
    }

    private RawImage[] GetImages()
    {
        //get images
        return new RawImage[totalImageCount];
    }
}
