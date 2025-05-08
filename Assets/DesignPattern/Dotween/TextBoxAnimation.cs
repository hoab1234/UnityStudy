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
        // �ʱ� ������ ����
        originalLineSize = horizontalLine.sizeDelta;
        originalContainerPos = image.transform.position;

        // �ʱ� ����
        horizontalLine.sizeDelta = new Vector2(0, originalLineSize.y);
    }

    private void Start()
    {

        // �ִϸ��̼� ����
        containerButton.onClick.AddListener(() => StartAnimation());
    }

    public void StartAnimation()
    {
        ResetAnimation();
        StartCoroutine(AnimationSequence());
    }

    private IEnumerator AnimationSequence()
    {
        // 1. ���� ���� ���ʿ��� ���������� �þ
        horizontalLine.sizeDelta = new Vector2(0, originalLineSize.y);
        horizontalLine.DOSizeDelta(originalLineSize, lineGrowDuration)
            .SetEase(lineEase);

        yield return new WaitForSeconds(lineGrowDuration);

        image.rectTransform.DOMoveY(2080, 0.1f).SetEase(Ease.InOutSine);
    }

    // �ִϸ��̼� ���� �޼��� (�ʿ��� ���)
    public void ResetAnimation()
    {
        horizontalLine.sizeDelta = new Vector2(0, originalLineSize.y);
        image.transform.position = originalContainerPos;
    }
}