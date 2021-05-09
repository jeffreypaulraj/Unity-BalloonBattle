using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManagerScript : MonoBehaviourPunCallbacks
{    
    

    void Start()
    {
    }

    public override void OnConnectedToMaster(){
        Debug.Log("Connected to Master Server");
    }

    public override void OnCreatedRoom(){
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }

    public void JoinOrCreateRoom(string roomName)
    {
        PhotonNetwork.JoinOrCreateRoom(roomName, null, null, null);
    }
    public void ChangeScene(string sceneName){
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void Update()
    {
        
    }
}
