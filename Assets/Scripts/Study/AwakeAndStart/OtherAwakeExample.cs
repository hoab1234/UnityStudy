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
            Debug.Log("Awake�Լ��� �������� ȣ��Ǳ� ������ awake�Լ����� �ʱ�ȭ�Ǵ� ���� ������ �� �����ϴ�.");
        }
    }
}