using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageGenerator : MonoBehaviour
{
    [SerializeField] private List<Transform> generatePointTransList;
    [SerializeField] private ImageObject imagePrefab;
    
    [Space(20)]
    [Header("Customize Image Property")]
    [SerializeField] private float imageMovingSpeed = 10f;
    [SerializeField] private float generateTermTime = 1f;

    private List<ImageObject> objects = new List<ImageObject>();

    public void Init(Texture[] textures)
    {
        ResetPreviousResult();
        StartCoroutine(GenerateImageObjects(textures));
    }

    IEnumerator GenerateImageObjects(Texture[] textures)
    {
        for (int i = 0; i < textures.Length; i++)
        {
            ImageObject imageQuad = Instantiate(imagePrefab, generatePointTransList[Random.Range(0, generatePointTransList.Count)]);
            objects.Add(imageQuad);

            imageQuad.Init(textures[i], imageMovingSpeed);

            yield return new WaitForSeconds(generateTermTime); ;
        }
    }

    private void ResetPreviousResult()
    {
        if (objects != null)
        {
            foreach (var obj in objects)
            {
                Destroy(obj);
            }
            objects.Clear();
        }
    }
}
