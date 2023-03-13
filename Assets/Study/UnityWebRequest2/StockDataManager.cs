using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StockDataManager : MonoBehaviour
{
    private const string BASE_URL = "https://fchart.stock.naver.com/sise.nhn?symbol=&timeframe=day&count=100&requestType=0";

    private string tempCode = "005930";

    private void Start()
    {
        
    }

    //��û
    IEnumerator GetStorkData()
    {
        UnityWebRequest request = UnityWebRequest.Get(GenerateUrl(tempCode));
        
        yield return request.SendWebRequest();
        Debug.Log(request.result);
    }

    private string GenerateUrl(string stockCode)
    {
        string url = string.Empty;
        // �ֽ� ���� �ڵ�� �Բ� ���ڿ� �簡���ؾ���
        return url; 
    }
}