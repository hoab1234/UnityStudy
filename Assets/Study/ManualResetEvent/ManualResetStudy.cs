using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ManualResetStudy : MonoBehaviour
{
    ManualResetEvent manualEvent = new ManualResetEvent(false);

    private void Start()
    {
        for(int i = 0; i< 10; i++)
        {
            new Thread(Run).Start(i);
        }

        Invoke("ManualEventSet", 3);
    }

    private void ManualEventSet() => manualEvent.Set();

    private void Run(object id)
    {
        Debug.Log($"{id} in wait");
        // ManualResetEvent ��ȣ ���
        manualEvent.WaitOne();
        Debug.Log($"{id} Done");
    }
}

/* ManualResetEvent�� �ϳ��� �����常 �����Ű�� �ݴ� AutoResetEvent�� �޸�,
 * �ѹ� ������ ��� ���̴� ��� �����带 �����ϰ� �ϰ�, �ڵ忡�� ��������
 * Reset()�� ȣ���Ͽ� ���� �ݰ� ���� ������ ��������� �ٽ� ������ �Ѵ�.
 * 
 * ���� �������� ������ ������Ų ��, ManualResetEvent�� ��ȣ�� ����
 * ������̴� ��� ��������� �Ѳ����� ������Ѻ���.
 * 
 * 
 */