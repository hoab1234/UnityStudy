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
        // ManualResetEvent 신호 대기
        manualEvent.WaitOne();
        Debug.Log($"{id} Done");
    }
}

/* ManualResetEvent는 하나의 쓰레드만 통과시키고 닫는 AutoResetEvent와 달리,
 * 한번 열리면 대기 중이던 모든 쓰레드를 실행하게 하고, 코드에서 수동으로
 * Reset()을 호출하여 문을 닫고 이후 도착한 쓰레드들을 다시 대기토록 한다.
 * 
 * 여러 쓰레드의 실행을 중지시킨 후, ManualResetEvent로 신호를 보내
 * 대기중이던 모든 쓰레드들을 한꺼번에 실행시켜보자.
 * 
 * 
 */
