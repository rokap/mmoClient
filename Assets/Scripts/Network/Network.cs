using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{
    
    public static SocketIOComponent socket;

    void Start()
    {
        socket = GameObject.FindObjectOfType<SocketIOComponent>();
        socket.Connect();
        socket.On("error", Error);
    }

    void Error(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public static void AddListener(string cmd, System.Action<SocketIOEvent> callback )
    {
        socket.On(cmd, callback);
    }

    public static void Send(string cmd)
    {

        if (socket.IsConnected)
            socket.Emit(cmd);
        else
            Debug.LogError("No Connection to Server");
    }

    public static void Send(string cmd, JSONObject jsonObject)
    {
        if (socket.IsConnected)
            socket.Emit(cmd, jsonObject);
        else
            Debug.LogError("No Connection to Server");
    }

    public static void Send(string cmd, string str)
    {
        
        if (socket.IsConnected)
            socket.Emit(cmd, str);
        else
            Debug.LogError("No Connection to Server");
    }

    public static void Send(string cmd, System.Action<JSONObject> action)
    {
        if (socket.IsConnected)
            socket.Emit(cmd, action);
        else
            Debug.LogError("No Connection to Server");
    }

    public static void Send(string cmd, JSONObject jsonObject, System.Action<JSONObject> action)
    {
        if (socket.IsConnected)
            socket.Emit(cmd, jsonObject, action);
        else
            Debug.LogError("No Connection to Server");
    }

    public static void RemoveListener(string cmd, System.Action<SocketIOEvent> callback)
    {
        socket.Off(cmd, callback);
    }

    public static string GetNetId()
    {
        return socket.sid;
    }
}