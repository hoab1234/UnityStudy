using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Client101 : MonoBehaviour
{

    private const int PORT = 1234;
    private Socket clientSocket;

    private void Start()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(new IPEndPoint(IPAddress.Loopback, PORT));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            byte[] buffer = Encoding.ASCII.GetBytes("Hello Server!");
            clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
    }

    private void OnApplicationQuit()
    {
        clientSocket.Close();
    }
}
