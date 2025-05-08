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
        Debug.Log("�񵿱� �۾� ����");
        await Task.Delay(2000); // 2�� ���

        Debug.Log("�񵿱� �۾� �Ϸ�");
    }


    // �ַ� �̺�Ʈ �ڵ鷯���� ��� (��ȯ���� ����)
    public async void MyAsyncVoidFunc()
    {
        await Task.Delay(1000);
        Debug.Log("�񵿱� �۾� �Ϸ�");
    }
    // ��ȯ���� ���� �񵿱� �۾�
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
            Debug.LogError($"���� �߻�: {ex.Message}");
        }
    }

    public async Task PotentiallyFailingMethodAsync()
    {
        await Task.Delay(1000);
        throw new Exception("�񵿱� �۾� �� ���� �߻�");
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
