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
    public Camera mainCamera;
    public Camera cameraOne;
    public Camera cameraTwo;

    public static CarMovementScript instance;

    void Awake(){
        if (instance != null && instance != this){
            gameObject.SetActive(false);
        }
        else {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
    }

    void Start(){
        //PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        cars = new GameObject[2];
        cars[0] = carOne;
        cars[1] = carTwo;

        int count = 0;
        foreach(Player player in PhotonNetwork.PlayerList){
            if (player.Equals(PhotonNetwork.LocalPlayer)) {
                playerIndex = count;
            }
            count++;
        }
        mainCamera.enabled = false;
        if(playerIndex == 0) {
            cameraTwo.enabled = false;
        }
        else
        {
            cameraOne.enabled = false;
        }
    }

    void Update(){
        //PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        ExitGames.Client.Photon.Hashtable propertyHash = new ExitGames.Client.Photon.Hashtable();
        propertyHash.Add("Position", cars[playerIndex].transform.position);
        propertyHash.Add("Rotation", cars[playerIndex].transform.rotation);
        PhotonNetwork.LocalPlayer.SetCustomProperties(propertyHash);

        int count = 0;
        foreach(Player player in PhotonNetwork.PlayerList){
            if (!player.Equals(PhotonNetwork.LocalPlayer)){
                cars[count].transform.position = (Vector3)player.CustomProperties["Position"];
                cars[count].transform.rotation = (Quaternion)player.CustomProperties["Rotation"];
            }
            count++;

        }

    }
    private void FixedUpdate(){
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        breaking = Input.GetKey(KeyCode.Space);
        //applyMovement(cars[playerIndex]);

        if (Input.GetKey(KeyCode.LeftArrow)){
            cars[playerIndex].transform.Translate(Vector3.left);
            //if(cars[playerIndex].transform.rotation.eulerAngles.x > -90){
            //    cars[playerIndex].transform.Rotate(new Vector3(0, -1, 0));
            //}
        }
        else if (Input.GetKey(KeyCode.RightArrow)){
            cars[playerIndex].transform.Translate(Vector3.right);
            cars[playerIndex].transform.Rotate(new Vector3(0,0,1));
        }
        else if (Input.GetKey(KeyCode.UpArrow)){
            cars[playerIndex].transform.Translate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.DownArrow)){
            cars[playerIndex].transform.Translate(Vector3.back);
        }
        Rigidbody rb = this.gameObject.transform.GetComponent<Rigidbody>();

    }
    void applyMovement(GameObject car)
    {

    }
}
