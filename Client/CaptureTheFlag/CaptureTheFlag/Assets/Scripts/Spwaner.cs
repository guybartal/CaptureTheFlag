using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SocketIO;
using Assets.Scripts;
using UnityEngine.UI;

public class Spwaner : MonoBehaviour
{
    public GameObject myPlayer;
    public GameObject playerPrefab;
    public SocketIOComponent socket;
    public Material redMaterial;
    public Material blueMaterial;
    Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    public GameObject SpwanSplayer(Player player)
    {
        try
        {
            Vector3 pos = new Vector3(player.x, 0, player.y);
            var playerObject = Instantiate(playerPrefab, pos, Quaternion.identity) as GameObject;

            SetPlayerData(playerObject, player);
            AddPlayer(player.id, playerObject);

            return playerObject;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
        return null;
    }

    public GameObject FindPlayer(string id)
    {
        if (players.ContainsKey(id))
        {
            return players[id];
        }
        else
        {
            return null;
        }
    }

    public void AddPlayer(string id, GameObject player)
    {
        players.Add(id, player);
    }

    public void Remove(string id)
    {
        var player = FindPlayer(id);
        Destroy(player);
        players.Remove(id);
    }

    public void SetPlayerData(GameObject playerGameObject, Player player)
    {
        playerGameObject.GetComponent<NetworkEntity>().Player.id = player.id;
        playerGameObject.GetComponent<NetworkEntity>().Player.name = player.name;
        playerGameObject.GetComponent<NetworkEntity>().Player.team = player.team;
        playerGameObject.transform.FindChild("PlayerName").GetComponent<TextMesh>().text = player.name;
        SkinnedMeshRenderer rend = playerGameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        if (rend != null)
        {
            Debug.Log("replacing material to " + (player.team == Team.Red ? redMaterial.GetInstanceID().ToString() : blueMaterial.GetInstanceID().ToString()));
            Material[] matArr = new Material[1] { player.team == Team.Red ? redMaterial : blueMaterial };
            rend.materials = matArr;
        }
        else
        {
            Debug.Log("SkinnedMeshRenderer not found");
        }
    }
}
