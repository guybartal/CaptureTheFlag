using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class ClickMove : MonoBehaviour, IClickable
{
    public GameObject player;

    public void OnClick(RaycastHit hit)
    {
       if (GameManager.Instance.GameState == GameState.Running)
        {
            var navPos = player.GetComponent<Navigator>();
            navPos.NavigateTo(hit.point);
            Network.Move(hit.point);
        }
    }
}
