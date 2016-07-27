using UnityEngine;
using System.Collections;
using System;

public class ClickFollow : MonoBehaviour, IClickable
{

    public GameObject myPlayer;
    public NetworkEntity networkEntity;
    Targeter targeter;

    public void Start()
    {
        networkEntity = GetComponent<NetworkEntity>();
        targeter = myPlayer.GetComponent<Targeter>();
    }

    public void OnClick(RaycastHit hit)
    {
        Debug.Log("following " + hit.collider.gameObject.name);

        Network.Follow(networkEntity.Player.id);

        targeter.target = transform;
    }
}
