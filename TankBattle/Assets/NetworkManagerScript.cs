﻿using System.Collections;
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
        //PhotonNetwork.JoinOrCreateRoom("testRoom",null,null,null);
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
