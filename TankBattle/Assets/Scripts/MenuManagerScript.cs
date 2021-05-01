using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MenuManagerScript : MonoBehaviourPunCallbacks
{
    public Button joinCreateButton;
    public Button startButton;
    public Button leaveButton;

    public InputField codeInputField;
    public InputField nameField;
    public Text playerCountText;

    public Text playerListText;
    public Text roomCodeText;

    public Button roomNextButton;
    public Button playerNextButton;

    public bool isHost;

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
        PhotonNetwork.AutomaticallySyncScene = true;
        joinCreateButton.onClick.AddListener(JoinCreateRoom);
        startButton.onClick.AddListener(StartGameCheck);
        roomNextButton.onClick.AddListener(nextRoom);
        playerNextButton.onClick.AddListener(nextPlayer);
        leaveButton.onClick.AddListener(LeaveMethod);
        resetMenu();
    }

    public override void OnConnectedToMaster(){
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom(){
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.CurrentRoom.IsVisible = true;
        playerList = new List<string>();
        playerList.Add("Players");
        playerList.Add(PhotonNetwork.LocalPlayer.NickName);
        playerCount = 1;
        isHost = true;
        playerCountText.text = "Number of Players in Room: " + playerCount;
        gameCreated = true;

    }

    public override void OnJoinedRoom(){
        playerList = new List<string>();
        playerList.Add("Players");
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.CurrentRoom.IsVisible = true;
        //for (int i = 0; i < PhotonNetwork.CurrentRoom.Players.Count; i++){
        foreach (KeyValuePair<int, Player> entry in PhotonNetwork.CurrentRoom.Players) {
            playerList.Add(entry.Value.NickName);
        }
        playerCount = PhotonNetwork.CountOfPlayers;
        playerCountText.text = "Number of Players in Room: " + playerCount;
        gameCreated = true;

    }

    public override void OnPlayerEnteredRoom(Player newPlayer){
        playerList.Add(newPlayer.NickName);
        playerCount++;
        playerCountText.text = "Number of Players in Room: " + playerCount;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        base.OnRoomListUpdate(roomList);
        Debug.Log("Room Update");
        roomsList = new List<string>();
        roomsList.Add("Rooms Available");

        foreach(Photon.Realtime.RoomInfo room in roomList)
        {
            roomsList.Add(room.Name);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        if (playerList.Contains(otherPlayer.NickName)){
            playerList.Remove(otherPlayer.NickName);
            playerCount--;
            playerCountText.text = "Number of Players in Room: " + playerCount;
            if(playerCount == 1){
                isHost = true;
            }
        }
    }

    public override void OnJoinedLobby(){
        Debug.Log("Joined lobby");
        if (!PhotonNetwork.InLobby){
            resetMenu();
        }

    }

    public void JoinOrCreateRoom(string roomName) {
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions {IsVisible = true}, null, null);
        PhotonNetwork.LocalPlayer.NickName = nameField.text;
        
    }

    public void nextRoom(){
        roomDisplayCount++;
        if(roomDisplayCount >= roomsList.Count){
            roomDisplayCount = 0;
        }
        roomCodeText.text = roomsList[roomDisplayCount];
        Debug.Log("Method 1 Called " + roomDisplayCount);
    }

    public void nextPlayer(){
        playerDisplayCount++;
        if(playerDisplayCount >= playerList.Count){
            playerDisplayCount = 0;
        }
        playerListText.text = playerList[playerDisplayCount];
        Debug.Log("Method 2 Called " + playerDisplayCount);
    }

    public void ChangeScene() {
        PhotonNetwork.LoadLevel("Game");
    }

    public void StartGameCheck() {
        if (gameCreated && isHost) {
            ChangeScene();
        }
    }

    public void LeaveMethod(){
        if (gameCreated){
            Debug.Log("leave function called");
            PhotonNetwork.LeaveRoom();
            resetMenu();
            gameCreated = false;
        }
    }

    public void resetMenu() {
        Debug.Log("reset Function called");
        PhotonNetwork.JoinLobby();
        gameCreated = false;
        playerCount = 0;

        playerList = new List<string>();
        roomsList = new List<string>();

        playerList.Add("Players");
        roomsList.Add("Rooms Available");
        playerDisplayCount = 0;
        roomDisplayCount = 0;

        playerListText.text = "Players";
        roomCodeText.text = "Rooms Available";
        isHost = false;

        codeInputField.text = "";
        nameField.text = "";
        playerCountText.text = "Number of Players in Room: ";

    }

    void Update(){
       
    }

    void JoinCreateRoom(){
        string code = codeInputField.text;
        JoinOrCreateRoom(code);
    }
}
