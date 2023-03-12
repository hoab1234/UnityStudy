using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageGenerator : MonoBehaviour
{
    /*
     * 1. 데이터를 캐싱해서 사용하는 것은 좋지만, 실질적으로 의미가 있는지,
     * 유의미하지 않다면 오히려 가독성을 해치는지 생각해볼 것. 이번엔 waitforseconds 캐싱해서 썻었음
     * 마찬가지로 필요없는 지역변수는 아닌지 고민
     * 
     * 2. Instantiate 를 GameObject만 받아서 할 필요 없음
     * 타입을 바로 받아서 생성 가능
     */
    [SerializeField] private List<Transform> generatePointTransList;
    [SerializeField] private ImageObject imagePrefab;
    [SerializeField] private ImageLoader imageLoader;

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

    /*
     * SetSpeed, SetImage 처럼 설정하는 것보다 기존에 init을 썻다면 데이터 세팅하는데 
     * 그대로 init을 쓰고, 매개변수로 speed, image를 준다면 일관성,가독성 다 챙길 수 있다.
     * 굳이 for문을 쓰지않고 foreach를 써도 가독성이 더 좋을 것 같다.
     */
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
