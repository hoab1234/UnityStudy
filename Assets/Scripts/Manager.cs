using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    //공부 내용을 프리팹 하나에서 실행되도록 구현, 매니저에서 해당 프리팹을 인스턴트화해서 실행
    public string StudyPrefabName;

    [SerializeField] private Button ChangeStudyPrefab;

    private void Awake()
    {
        ChangeStudyPrefab.onClick.AddListener(InstantiateStudyPrefab);
    }
    
    private void InstantiateStudyPrefab() => Instantiate(Resources.Load(StudyPrefabName));
}
