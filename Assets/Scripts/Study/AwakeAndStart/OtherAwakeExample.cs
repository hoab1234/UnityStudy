using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherAwakeExample : MonoBehaviour
{
    [SerializeField] private AwakeExample awakeExample;

    private void Awake()
    {
        try
        {
            int i;
            i = awakeExample.number;
        }
        catch(NullReferenceException e)
        {
            Debug.Log("e");
            Debug.Log("Awake함수는 무작위로 호출되기 때문에 awake함수에서 초기화되는 값을 보장할 수 없습니다.");
        }
    }
}
