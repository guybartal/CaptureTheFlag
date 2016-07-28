using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SocketIO;
using Assets.Scripts;
using UnityEngine.UI;

public class Spwaner : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject flagPrefab;

    public SocketIOComponent socket;
    public Material redPlayerMaterial;
    public Material bluePlayerMaterial;
    Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    public Material redFlagMaterial;
    public Material blueFlagMaterial;
    Dictionary<string, GameObject> flags = new Dictionary<string, GameObject>();
    int floorSize = 40;

    public GameObject SpwanSplayer(Player player)
    {
        try
        {
            Vector3 pos = new Vector3(player.x, 0, player.y);
            var playerObject = Instantiate(playerPrefab, pos, Quaternion.identity) as GameObject;

            SetPlayerData(playerObject, player);
            AddPlayer(player.id, playerObject);
            
            
            Vector3 flagPos = new Vector3(player.flagX, 0, player.flagY);
            Debug.Log("flagPos " + JsonUtility.ToJson(flagPos));
            var flagObject = Instantiate(flagPrefab, flagPos, Quaternion.identity) as GameObject;

            SetFlagData(flagObject, player);
            AddFlag(player.id, flagObject);

            return playerObject;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
        return null;
    }

    public void SetFlagData(GameObject flagObject, Player player)
    {
        Vector3 flagPos = new Vector3(player.flagX, 0, player.flagY);
        flagObject.transform.position = flagPos;

        MeshRenderer rend = flagObject.GetComponent<MeshRenderer>();
        if (rend != null)
        {
            Debug.Log("replacing material to " + (player.team == Team.Red ? redFlagMaterial.GetInstanceID().ToString() : blueFlagMaterial.GetInstanceID().ToString()));
            Material[] matArr = new Material[1] { player.team == Team.Red ? redFlagMaterial : blueFlagMaterial };
            rend.materials = matArr;
        }
        else
        {
            Debug.Log("Renderer not found");
        }
    }

    public void AddFlag(string id, GameObject flagObject)
    {
        flags.Add(id, flagObject);
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
            Debug.Log("replacing material to " + (player.team == Team.Red ? redPlayerMaterial.GetInstanceID().ToString() : bluePlayerMaterial.GetInstanceID().ToString()));
            Material[] matArr = new Material[1] { player.team == Team.Red ? redPlayerMaterial : bluePlayerMaterial };
            rend.materials = matArr;
        }
        else
        {
            Debug.Log("SkinnedMeshRenderer not found");
        }
    }
}
