using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxAnimation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform horizontalLine;
    public Image image;
    public Button containerButton;

    [Header("Animation Settings")]
    [SerializeField] private float lineGrowDuration = 0.5f;
    [SerializeField] private float boxRiseDuration = 0.7f;
    [SerializeField] private float textFadeDuration = 0.3f;
    [SerializeField] private Ease lineEase = Ease.OutQuad;
    [SerializeField] private Ease boxEase = Ease.OutCubic;

    private Vector2 originalLineSize;
    private Vector3 originalContainerPos;

    private void Awake()
    {
        // 초기 사이즈 저장
        originalLineSize = horizontalLine.sizeDelta;
        originalContainerPos = image.transform.position;

        // 초기 설정
        horizontalLine.sizeDelta = new Vector2(0, originalLineSize.y);
    }

    private void Start()
    {

        // 애니메이션 시작
        containerButton.onClick.AddListener(() => StartAnimation());
    }

    public void StartAnimation()
    {
        ResetAnimation();
        StartCoroutine(AnimationSequence());
    }

    private IEnumerator AnimationSequence()
    {
        // 1. 가로 선이 왼쪽에서 오른쪽으로 늘어남
        horizontalLine.sizeDelta = new Vector2(0, originalLineSize.y);
        horizontalLine.DOSizeDelta(originalLineSize, lineGrowDuration)
            .SetEase(lineEase);

        yield return new WaitForSeconds(lineGrowDuration);

        image.rectTransform.DOMoveY(2080, 0.1f).SetEase(Ease.InOutSine);
    }

    // 애니메이션 리셋 메서드 (필요한 경우)
    public void ResetAnimation()
    {
        horizontalLine.sizeDelta = new Vector2(0, originalLineSize.y);
        image.transform.position = originalContainerPos;
    }
}