using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;


/*ChatServer 클래스 구조에 대한 설명

WebSocketServer

WebSocket 프로토콜을 사용하는 서버를 관리하는 클래스
클라이언트의 연결 요청을 받고 관리하는 역할


ChatService

WebSocketBehavior를 상속받아 실제 WebSocket 통신을 처리
각 클라이언트 연결마다 하나의 인스턴스가 생성됨
Sessions: 연결 상태의 모든 클라이언트를 관리하는 객체*/

public class ChatServer : MonoBehaviour
{
    // WebSocketServer 인스턴스를 저장할 변수
    private WebSocketServer wssv;
    // 서버가 사용할 포트 번호
    private const int PORT = 7890;

    void Awake()
    {
        // WebSocket 서버 인스턴스 생성
        // ws://localhost:7890 형태의 주소로 서버 생성
        wssv = new WebSocketServer($"ws://localhost:{PORT}");

        // '/chat' 경로로 들어온 연결에 대해 ChatService가 처리하도록 설정
        wssv.AddWebSocketService<ChatService>("/chat");
        
        // 서버 시작
        wssv.Start();
        Debug.Log($"Chat Server started on ws://localhost:{PORT}/chat");
    }

    // 게임 오브젝트가 파괴될 때 서버를 안전하게게 종료
    void OnDestroy()
    {
        if (wssv != null)
        {
            wssv.Stop();
        }
    }
}

// WebSocket 서비스를 처리하는 클래스
public class ChatService : WebSocketBehavior
{
    // 클라이언트로부터 메시지를 받았을 때 실행되는 메서드
    protected override void OnMessage(MessageEventArgs e)
    {
        // 받은 메시지를 연결된 모든 클라이언트에게 전송
        // e.Data: 클라이언트가 보낸 메시지 내용
        Sessions.Broadcast(e.Data);
    }
}