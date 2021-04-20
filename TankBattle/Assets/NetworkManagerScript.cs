using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManagerScript : MonoBehaviourPunCallbacks
{
    public static NetworkManagerScript instance;
    void Awake(){
        if(instance!=null && instance != this){
            gameObject.SetActive(false);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start(){
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){
        Debug.Log("Connected to Master Server");
        CreateRoom("testRoom");
    }

    public override void OnCreatedRoom(){
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public void CreateRoom(string roomName){
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName){
        PhotonNetwork.JoinRoom(roomName);
    }

    public void ChangeScene(string sceneName){
        PhotonNetwork.LoadLevel(sceneName);
    }
}
