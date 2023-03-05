using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageGenerator : MonoBehaviour
{
    [SerializeField] private List<Transform> generatePointTransList;
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private ImageLoader imageLoader;

    [Space(20)]
    [Header("Customize Image Property")]
    [SerializeField] private float imageQuadSpeed = 10f;
    [SerializeField] private float generateTermTime = 1f;

    private WaitForSeconds waitForGenerate;
    private int TotalImageCount = 10;
    public Texture[] Textures;
    public List<GameObject> Objects = new List<GameObject>();

    public void Init(Texture[] textures)
    {
        this.Textures = textures;
        TotalImageCount = textures.Length;
        waitForGenerate = new WaitForSeconds(generateTermTime);

        StartCoroutine(GenerateImageObjects());
    }

    IEnumerator GenerateImageObjects()
    {
        
        for (int i = 0; i < TotalImageCount ; i++)
        {
            GameObject imageQuad = Instantiate(imagePrefab, generatePointTransList[Random.Range(0, generatePointTransList.Count)]);
            Objects.Add(imageQuad);
            ImageObject imageObject = imageQuad.GetComponent<ImageObject>();

            imageObject.SetImage(Textures[i]);
            imageObject.SetSpeed(imageQuadSpeed);

            yield return waitForGenerate;
        }
    }
}
