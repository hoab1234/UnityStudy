using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private ImageGroupMotivator imageMotivateManager;
    private WaitForSeconds waitForGenerate;

    private void Awake()
    {
        waitForGenerate = new WaitForSeconds(generateTermTime);
    }

    IEnumerator Start()
    {
        for(int i = 0; i < totalImageCount ; i++)
        {
            GameObject imageQuad = Instantiate(imagePrefab, generatePointTransList[Random.Range(0, generatePointTransList.Count)]);
            ImageObject imageObject = imageQuad.GetComponent<ImageObject>();
            imageObject.SetSpeed(imageQuadSpeed);

            yield return waitForGenerate;
        }
    }
}
