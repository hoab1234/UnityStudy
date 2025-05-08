using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncExample : MonoBehaviour
{
    void Start()
    {
        MyAsyncVoidFunc();
    }

    public async void DoSomethingAsync()
    {
        Debug.Log("비동기 작업 시작");
        await Task.Delay(2000); // 2초 대기

        Debug.Log("비동기 작업 완료");
    }


    // 주로 이벤트 핸들러에서 사용 (반환값이 없음)
    public async void MyAsyncVoidFunc()
    {
        await Task.Delay(1000);
        Debug.Log("비동기 작업 완료");
    }
    // 반환값이 없는 비동기 작업
    public async Task MyAsyncTaskFunc()
    {
        await Task.Delay(1000);
    }

    public async Task<int> MyAsyncResultFunc()
    {
        await Task.Delay(1000);
        return 42;
    }

    public async Task ExceptionHandlingExample()
    {
        try
        {
            await PotentiallyFailingMethodAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError($"오류 발생: {ex.Message}");
        }
    }

    public async Task PotentiallyFailingMethodAsync()
    {
        await Task.Delay(1000);
        throw new Exception("비동기 작업 중 오류 발생");
    }

    public async Task<string> DownloadWebsiteContentAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            string content = await client.GetStringAsync(url);
            return content;
        }
    }

    public async Task SaveToFileAsync(string filePath, string content)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            await writer.WriteAsync(content);
        }
    }

}
