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

    public Text playerListText;
    public Text roomCodeText;

    public Button roomNextButton;
    public Button playerNextButton;

    public static MenuManagerScript instance;
    bool gameCreated;
    int playerCount;

    int playerDisplayCount;
    int roomDisplayCount;

    List<string> playerList;
    List<string> roomsList;

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
        roomNextButton.onClick.AddListener(nextRoom);
        playerNextButton.onClick.AddListener(nextPlayer);
        gameCreated = false;
        playerCount = 0;

        playerList = new List<string>();
        roomsList = new List<string>();

        playerList.Add("Players");
        roomsList.Add("Rooms Available");

        playerDisplayCount = 0;
        roomDisplayCount = 0;
    }

    public override void OnCreatedRoom(){
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        playerList.Add(newPlayer.NickName);
        playerCount = PhotonNetwork.CountOfPlayers;
        playerCountText.text = "Number of Players in Room: " + playerCount;
    }

    public override void OnRoomListUpdate(List<Photon.Realtime.RoomInfo> roomList)
    {
        roomsList = new List<string>();
        roomsList.Add("Rooms Available");

        foreach(Photon.Realtime.RoomInfo room in roomList)
        {
            roomsList.Add(room.Name + " (" + room.PlayerCount + " Players)");
        }
    }

    public void JoinOrCreateRoom(string roomName) {
        PhotonNetwork.JoinOrCreateRoom(roomName, null, null, null);
        PhotonNetwork.LocalPlayer.NickName = nameField.text;
        playerCount = PhotonNetwork.CountOfPlayers;
        playerCountText.text = "Number of Players in Room: " + playerCount;
        gameCreated = true;
    }

    public void nextRoom()
    {
        Debug.Log("Method 1 Called");
        roomDisplayCount++;
        if(roomDisplayCount >= roomsList.Count){
            roomDisplayCount = 0;
        }
        roomCodeText.text = roomsList[roomDisplayCount];
    }

    public void nextPlayer()
    {
        Debug.Log("Method 2 Called");
        playerDisplayCount++;
        if(playerDisplayCount >= playerList.Count){
            playerDisplayCount = 0;
        }
        playerListText.text = playerList[playerDisplayCount];
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
            
            string playerListString = "";
            //foreach (var player in PhotonNetwork.PlayerList) {
            //    playerListString += player.NickName + "\n";
            //}
            //playerList.text = playerListString;

        }
    }

    void JoinCreateRoom(){
        string code = codeInputField.text;
        JoinOrCreateRoom(code);
    }
}
