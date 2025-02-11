using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using TMPro;
using System.Collections.Generic;
using System;

public class ChatClient : MonoBehaviour
{
    // WebSocket 서버와 통신하는 객체
    private WebSocket ws;
    // 서버 주소
    private const string SERVER_URL = "ws://localhost:7890/chat";
    // 메시지 큐: 스레드 간 안전한 메시지 전달을 위한 버퍼
    private Queue<string> messageQueue = new Queue<string>();

    // UI 컴포넌트 참조
    public TMP_InputField messageInput;  // 메시지 입력 필드
    public TMP_Text chatDisplay;         // 채팅 내용 표시 영역
    public ScrollRect scrollRect;

    private bool isConnected = false;  // 연결 상태를 저장하는 변수 추가

    void Start()
    {
        ConnectToServer();
        SetupUI();
    }

    // 서버에 연결하는 메서드
    void ConnectToServer()
    {
        ws = new WebSocket(SERVER_URL);

        // 서버 연결 성공 시 호출되는 이벤트 핸들러
        ws.OnOpen += (sender, e) => {
            Debug.Log("서버에 연결되었습니다");
            isConnected = true;
        };

        // 서버 연결 종료 시 호출되는 이벤트 핸들러  
        ws.OnClose += (sender, e) => {
            Debug.Log("서버와의 연결이 종료되었습니다");
            isConnected = false;
        };

        // 메시지 수신 시 호출되는 이벤트 핸들러
        ws.OnMessage += (sender, e) => {
            // 메시지 큐 동기화를 위한 락
            lock (messageQueue) {
                messageQueue.Enqueue(e.Data);
            }
        };

        // 에러 발생 시 호출되는 이벤트 핸들러
        ws.OnError += (sender, e) => {
            Debug.LogError($"웹소켓 에러: {e.Message}");
        };

        // 서버 연결 시도
        try {
            ws.Connect();
        }
        catch (Exception e) {
            Debug.LogError($"연결 에러: {e.Message}"); 
        }
    }

    // 매 프레임마다 실행되는 Unity 기본 메서드

    /*스레드 안전성
    
    WebSocket의 메시지 수신은 별도의 스레드에서 발생
    Unity의 UI 컴포넌트는 메인 스레드에서만 접근
    이를 해결하기 위해 Queue와 lock 사용 필요
    
    동시 접근 상황
    여러 클라이언트가 동시에 메시지를 보낼 수 있습니다
    각 메시지는 다른 스레드에서 처리될 수 있습니다
    각 스레드가 정확히 동시에 큐의 데이터를 접근하면 데이터가 손상될 수 있습니다

    한 스레드가 lock을 획득하면 다른 스레드는 해당 코드 영역을 실행할 수 없게 대기해야 합니다
    lock이 해제되면 모든 대기된 스레드가 실행될 수 있습니다
     */
    void Update()
    {
        // 큐에 있는 메시지를 처리
        lock (messageQueue)
        {
            while (messageQueue.Count > 0)
            {
                DisplayMessage(messageQueue.Dequeue());
            }
        }
    }

    // UI 이벤트 설정
    void SetupUI()
    {
        // 전송 버튼 클릭 이벤트 설정
        messageInput.onSubmit.AddListener(_ => SendChatMessage());
    }

    // 메시지 전송 메서드
    void SendChatMessage()
    {
        if (!isConnected)
        {
            Debug.LogWarning("Not connected to server!");
            return;
        }

        if (string.IsNullOrEmpty(messageInput.text)) return;

        try
        {
            ws.Send(messageInput.text);
            messageInput.text = "";
        }
        catch (Exception e)
        {
            Debug.LogError($"Send Error: {e.Message}");
        }
    }

    // 메시지를 화면에 표시하는 메서드
    void DisplayMessage(string message)
    {
        chatDisplay.text += $"\n{message}";

        // 다음 프레임에서 스크롤을 맨 아래로 이동
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    // 게임 오브젝트가 파괴될 때 연결 해제
    void OnDestroy()
    {
        if (ws != null)
        {
            ws.Close();
        }
    }
}