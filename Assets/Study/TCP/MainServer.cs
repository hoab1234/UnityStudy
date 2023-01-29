using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class MainServer : MonoBehaviour
{
    private const int PORT = 1234;
    private Socket serverSocket;

    private void Start()
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
        serverSocket.Listen(5);
        serverSocket.BeginAccept(new System.AsyncCallback(AcceptCallback), null);
    }

    private void AcceptCallback(System.IAsyncResult result)
    {
        Socket socket = serverSocket.EndAccept(result);
        byte[] buffer = new byte[1024];
        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new System.AsyncCallback(ReceiveCallback), socket);
        serverSocket.BeginAccept(new System.AsyncCallback(AcceptCallback), null);
    }

    private void ReceiveCallback(System.IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int received = socket.EndReceive(result);
        byte[] data = new byte[received];
        //Array.Copy(data, 0, received);
        string text = Encoding.ASCII.GetString(data);
        Debug.Log("Received: " + text);
        socket.BeginReceive(data, 0, data.Length, SocketFlags.None, new System.AsyncCallback(ReceiveCallback), socket);
    }

    private void OnApplicationQuit()
    {
        serverSocket.Close();
    }
}
