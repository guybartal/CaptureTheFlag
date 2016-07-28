using UnityEngine;
using System.Collections;
using SocketIO;
using System;
using System.Collections.Generic;
using Assets.Scripts;

public class Network : MonoBehaviour
{

    static SocketIOComponent socket;
    public GameObject myPlayer;
    public GameObject myFlag;
    public Spwaner spwaner;
    // Use this for initialization
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("register", OnRegister);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMove);
        socket.On("follow", OnFollow);
        socket.On("registered", OnRegistered);
        socket.On("disconnected", OnDisconnected);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);
    }

    public static void Login(Player player)
    {
        Debug.Log("login player" + JsonUtility.ToJson(player));
        socket.Emit("login", PlayerToJson(player));
    }

    public static void Move(Vector3 position)
    {
        // send pos to node
        Debug.Log("sending position to node" + Network.VectorToJson(position));
        socket.Emit("move", Network.VectorToJson(position));
    }

    public static void Follow(string id)
    {
        // send pos to node
        Debug.Log("sending follow player id" + Network.PlayerIdToJson(id));
        socket.Emit("follow", Network.PlayerIdToJson(id));
    }

    private void OnUpdatePosition(SocketIOEvent e)
    {
        var id = e.data["id"].str;
        Debug.Log("update player position " + e.data);
        var position = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "y"));

        var player = spwaner.FindPlayer(id);
        player.transform.position = position;
    }

    private void OnRequestPosition(SocketIOEvent e)
    {
        Debug.Log("server is requesting position");
        socket.Emit("updatePosition", VectorToJson(myPlayer.transform.position));

    }

    private void OnDisconnected(SocketIOEvent e)
    {
        Debug.Log("player disconnected " + e.data);
        var id = e.data["id"].str;
        spwaner.Remove(id);
    }

    private void OnRegistered(SocketIOEvent e)
    {
        Debug.Log("registered id" + e.data);
    }

    private void OnMove(SocketIOEvent e)
    {
        Debug.Log("Player is moving" + e.data);

        var pos = GetVectorFromJson(e);

        Debug.Log("pos: " + pos);

        var id = e.data["id"].str;
        var player = spwaner.FindPlayer(id);
        if (player == null)
        {
            Debug.LogError("player not found" + id);
        }
        else
        {
            Debug.Log("moving player " + player.name);

            var navigatePos = player.GetComponent<Navigator>();
            navigatePos.NavigateTo(pos);
        }
    }

    private void OnFollow(SocketIOEvent e)
    {
        Debug.Log("follow request" + e.data);

        var id = e.data["id"].str;
        var player = spwaner.FindPlayer(id);

        var targetId = e.data["id"].str;
        var targetTransform = spwaner.FindPlayer(targetId).transform;

        var target = player.GetComponent<Targeter>();
        target.target = targetTransform;
    }

    private void OnSpawned(SocketIOEvent e)
    {
        Debug.Log("spawned" + e.data);
        Player player = GetPlayerFromJson(e);

        var playerObject = spwaner.SpwanSplayer(player);

        if (e.data["x"]) // player was just spwaned with zero position
        {
            var moveToPosition = GetVectorFromJson(e);
            var navigatePos = playerObject.GetComponent<Navigator>();
            navigatePos.NavigateTo(moveToPosition);
        }
    }

    private static Player GetPlayerFromJson(SocketIOEvent e)
    {
        return new Player
        {
            id = e.data["id"].str,
            name = e.data["name"].str,
            team = (Team)(int)(e.data["team"].n),
            x = e.data["x"].n,
            y = e.data["y"].n
        };
    }

    private static Vector3 GetVectorFromJson(SocketIOEvent e)
    {
        return new Vector3(e.data["x"].n, 0, e.data["y"].n);
    }

    private void OnConnected(SocketIOEvent e)
    {
        Debug.Log("connected");
    }
    private void OnRegister(SocketIOEvent e)
    {
        Debug.Log("successfully registered, with id: " + e.data);
        Player player = GetPlayerFromJson(e);
        spwaner.SetPlayerData(myPlayer, player);
        spwaner.AddPlayer(player.id, myPlayer);
        spwaner.SetFlagData(myFlag, player);
        spwaner.AddFlag(player.id, myFlag);
    }

    private static float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].str);
    }

    public static JSONObject PlayerToJson(Player player)
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("id", player.id);
        j.AddField("name", player.name);
        j.AddField("team", (int)player.team);
        return j;
    }

    public static JSONObject VectorToJson(Vector3 vector)
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("x", vector.x);
        j.AddField("y", vector.z);
        return j;
    }

    public static JSONObject PlayerIdToJson(string id)
    {
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("targetId", id);
        return j;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
