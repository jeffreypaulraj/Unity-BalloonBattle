using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuManagerScript : MonoBehaviourPunCallbacks
{
    public Button joinCreateButton;
    public Button startButton;
    public InputField codeInputField;
    public InputField nameField;
    public Text playerCountText;
    public Text playerList;
    public static MenuManagerScript instance;
    bool gameCreated;
    int playerCount;

    void Awake() {
        if (instance != null && instance != this) {
            gameObject.SetActive(false);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        PhotonNetwork.ConnectUsingSettings();
        joinCreateButton.onClick.AddListener(JoinCreateRoom);
        startButton.onClick.AddListener(StartGameCheck);
        gameCreated = false;
        playerCount = 0;
    }

    public override void OnCreatedRoom(){
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public void JoinOrCreateRoom(string roomName) {
        PhotonNetwork.JoinOrCreateRoom(roomName, null, null, null);
        PhotonNetwork.LocalPlayer.NickName = nameField.text;
        gameCreated = true;
    }

    public void ChangeScene() {
        PhotonNetwork.LoadLevel("Game");
    }

    public void StartGameCheck()
    {
        if (gameCreated) {
            ChangeScene();
        }
    }

    void Update(){
        if (gameCreated){
            playerCount = PhotonNetwork.CountOfPlayers;
            playerCountText.text = "Number of Players in Room: " + playerCount;
            string playerListString = "";
            foreach (var player in PhotonNetwork.PlayerList) {
                playerListString += player.NickName + "\n";
            }
            playerList.text = playerListString;

        }
    }

    void JoinCreateRoom(){
        string code = codeInputField.text;
        JoinOrCreateRoom(code);
    }
}
