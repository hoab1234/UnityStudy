using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DotweenUIExample : MonoBehaviour
{
    public Transform createdImagesPoint;
    public Image image;
    public Button UpImageButton;
    public Button DownImageButton;
    public Button createImageGroupButton;
    
    private List<GameObject> imageList = new List<GameObject>();

    private void Awake()
    {
        createImageGroupButton.onClick.AddListener(CreateImageGroup);
        UpImageButton.onClick.AddListener(UpImage);
        DownImageButton.onClick.AddListener(DownImage);
    }
    
    void Start()
    {
        InitalizePopupImageHegith();
    }

    private void InitalizePopupImageHegith()
    {
    // 화면 높이의 절반 계산
        float screenHeight = Screen.height;
        float targetHeight = screenHeight * 0.5f;
        
        // RectTransform 컴포넌트 가져오기
        RectTransform rectTransform = image.rectTransform;
        
        // 높이 설정
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = targetHeight;
        rectTransform.sizeDelta = sizeDelta;
    }

    public void UpImage()
    {
        image.rectTransform.DOMoveY(image.rectTransform.position.y + (Screen.height * 0.5f), 0.1f).SetEase(Ease.InOutSine);
    }

    public void DownImage()
    {
        image.rectTransform.DOMoveY(image.rectTransform.position.y - (Screen.height * 0.5f), 0.1f).SetEase(Ease.InOutSine);
    }
    
     private void CreateImageGroup()
    {
        // 화면 중앙 위치 계산
        float screenCenterX = Screen.width * 0.5f;
        float screenCenterY = Screen.height * 0.5f;

        // 이미지 간격 설정 (200은 이미지 최종 크기)
        float spacing = 220f;
        float totalWidth = spacing * 4; // 5개 이미지의 전체 너비

        if(imageList.Count > 0)
        {
            foreach(var image in imageList)
            {
                Destroy(image);
            }
            imageList.Clear();
        }

        // 5개의 이미지 생성
        for (int i = 0; i < 5; i++)
        {
            // 이미지 게임오브젝트 생성
            GameObject imageObj = new GameObject($"Image_{i}");
            imageObj.transform.SetParent(createdImagesPoint);
            imageList.Add(imageObj);

            // UI 이미지 컴포넌트 추가 및 랜덤 색상 설정
            Image img = imageObj.AddComponent<Image>();
            img.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            
            // RectTransform 설정
            RectTransform rect = img.rectTransform;
            rect.anchoredPosition = new Vector2((spacing * i), 0);
            rect.sizeDelta = Vector2.zero; // 초기 크기 0으로 설정
            
            // 크기 애니메이션 시퀀스 생성
            float delay = 0.2f * i; 
            rect.DOSizeDelta(new Vector2(200f, 200f), 0.2f)
                .SetEase(Ease.OutBack)
                .SetDelay(delay);
        }
    }
}
