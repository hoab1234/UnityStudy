using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    //���� ������ ������ �ϳ����� ����ǵ��� ����, �Ŵ������� �ش� �������� �ν���Ʈȭ�ؼ� ����
    public string StudyPrefabName;
    [SerializeField] private Button ChangeStudyPrefab;

    private void Awake()
    {
        ChangeStudyPrefab.onClick.AddListener(InstantiateStudyPrefab);
    }
    
    private void InstantiateStudyPrefab() => Instantiate(Resources.Load(StudyPrefabName));
}