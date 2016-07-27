using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
   public GameState GameState = GameState.Login;
    public GameObject GUICanvas;
    public GameObject LoginPanel;
    public GameObject ChooseTeamPanel;
    public GameObject MyPlayer;
    public Text PlayerName;
    public Team Team;

    private GameManager()
    {

    }

    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            instance = GameManager.instance ?? FindObjectOfType<GameManager>();
            return instance;
        }
    }

    void ToggleMode()
    {
        switch (GameState)
        {
            case GameState.Login:
                GUICanvas.SetActive(true);
                MyPlayer.SetActive(false);
                LoginPanel.SetActive(true);
                ChooseTeamPanel.SetActive(false);
                break;
            case GameState.ChooseGroup:
                GUICanvas.SetActive(true);
                MyPlayer.SetActive(false);
                LoginPanel.SetActive(false);
                ChooseTeamPanel.SetActive(true);
                break;
            case GameState.Running:
                GUICanvas.SetActive(false);
                MyPlayer.SetActive(true);
                LoginPanel.SetActive(false);
                ChooseTeamPanel.SetActive(false);
                StartGame();
                break;
            case GameState.Ended:
                break;
            default:
                break;
        }
    }

    private void StartGame()
    {
        
    }

    public void Login()
    {
        GameState = GameState.ChooseGroup;
        ToggleMode();
    }

    public void ChooseGroup(int team)
    {
        this.Team = (Team)team;
        GameState = GameState.Running;
        Player player = new Player
            {
            name = this.PlayerName.text,
            team = this.Team
        };

        Network.Login(player);
        ToggleMode();
    }

	// Use this for initialization
	void Start ()
    {
        ToggleMode();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
