using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CarMovementScript : MonoBehaviourPunCallbacks
{
    float horizontalInput;
    float verticalInput;
    bool breaking;
    GameObject[] cars;
    public GameObject carOne;
    public GameObject carTwo;
    int playerIndex;

    public static CarMovementScript instance;

    void Awake(){
        if (instance != null && instance != this){
            gameObject.SetActive(false);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start(){
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        cars = new GameObject[PhotonNetwork.CountOfPlayers];
        cars[0] = carOne;
        cars[1] = carTwo;

        int count = 0;
        foreach(Player player in PhotonNetwork.PlayerList){
            if (player.Equals(PhotonNetwork.LocalPlayer)) {
                playerIndex = count;
            }
            count++;
        }
    }

    void Update(){
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        ExitGames.Client.Photon.Hashtable propertyHash = new ExitGames.Client.Photon.Hashtable();
        propertyHash.Add("Position", cars[playerIndex].transform.position);
        propertyHash.Add("Rotation", cars[playerIndex].transform.rotation);
        PhotonNetwork.LocalPlayer.SetCustomProperties(propertyHash);

        int count = 0;
        foreach(Player player in PhotonNetwork.PlayerList){
            cars[count].transform.position = (Vector3)player.CustomProperties["Position"];
            cars[count].transform.rotation = (Quaternion)player.CustomProperties["Rotation"];
            count++;
        }

    }
    private void FixedUpdate(){
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        breaking = Input.GetKey(KeyCode.Space);
        if (Input.GetKey(KeyCode.LeftArrow)){
            cars[playerIndex].transform.Translate(Vector3.left);
        }
        else if (Input.GetKey(KeyCode.RightArrow)){
            cars[playerIndex].transform.Translate(Vector3.right);
        }
        else if (Input.GetKey(KeyCode.UpArrow)){
            cars[playerIndex].transform.Translate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.DownArrow)){
            cars[playerIndex].transform.Translate(Vector3.back);
        }
        Rigidbody rb = this.gameObject.transform.GetComponent<Rigidbody>();

    }
}
