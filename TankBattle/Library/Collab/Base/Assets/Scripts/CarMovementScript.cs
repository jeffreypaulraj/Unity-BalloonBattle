using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

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
    public GameObject missileOne;
    public GameObject missileTwo;
    GameObject mainMissile;

    WheelCollider frontLeftCollider;
    WheelCollider frontRightCollider;
    WheelCollider rearLeftCollider;
    WheelCollider rearRightCollider;

    Transform frontLeftWheel;
    Transform frontRightWheel;
    Transform rearLeftWheel;
    Transform rearRightWheel;

    public float motorForce;
    float shootTimer;
    public float maxSteeringAngle;
    float steeringAngle;

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

        shootTimer = 0;

        Rigidbody rb1 = carOne.GetComponent<Rigidbody>();
        Rigidbody rb2 = carTwo.GetComponent<Rigidbody>();
        rb1.centerOfMass = new Vector3(rb1.centerOfMass.x, rb1.centerOfMass.y - 0.4f, rb1.centerOfMass.z);
        rb2.centerOfMass = new Vector3(rb2.centerOfMass.x, rb2.centerOfMass.y - 0.4f, rb2.centerOfMass.z);


        int count = 0;
        foreach(Player player in PhotonNetwork.PlayerList){
            if (player.Equals(PhotonNetwork.LocalPlayer)) {
                playerIndex = count;
            }
            count++;
        }
        //mainCamera.enabled = false;
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

        shootTimer += Time.deltaTime;

        int count = 0;
        foreach(Player player in PhotonNetwork.PlayerList) {
            if (!player.Equals(PhotonNetwork.LocalPlayer)) {
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
        applyMovement(cars[playerIndex]);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            shootProjectile();
        }
        
        //if (Input.GetKey(KeyCode.LeftArrow)){
        //    cars[playerIndex].transform.Translate(Vector3.left);
        //    //if(cars[playerIndex].transform.rotation.eulerAngles.x > -90){
        //    //    cars[playerIndex].transform.Rotate(new Vector3(0, -1, 0));
        //    //}
        //}
        //else if (Input.GetKey(KeyCode.RightArrow)){
        //    cars[playerIndex].transform.Translate(Vector3.right);
        //    cars[playerIndex].transform.Rotate(new Vector3(0,0,1));
        //}
        //else if (Input.GetKey(KeyCode.UpArrow)){
        //    cars[playerIndex].transform.Translate(Vector3.forward);
        //}
        //else if (Input.GetKey(KeyCode.DownArrow)){
        //    cars[playerIndex].transform.Translate(Vector3.back);
        //}
        //Rigidbody rb = this.gameObject.transform.GetComponent<Rigidbody>();

    }
    void applyMovement(GameObject car)
    {
        frontLeftCollider = car.transform.Find("WheelColliders").transform.Find("Tire_LF").GetComponent<WheelCollider>();
        frontRightCollider = car.transform.Find("WheelColliders").transform.Find("Tire_RF").GetComponent<WheelCollider>();
        rearLeftCollider = car.transform.Find("WheelColliders").transform.Find("Tire_LR").GetComponent<WheelCollider>();
        rearRightCollider = car.transform.Find("WheelColliders").transform.Find("Tire_RR").GetComponent<WheelCollider>();

        frontLeftWheel = car.transform.Find("Tires").transform.Find("Chev_LF");
        frontRightWheel = car.transform.Find("Tires").transform.Find("Chev_LF");
        rearLeftWheel = car.transform.Find("Tires").transform.Find("Chev_LF");
        rearRightWheel = car.transform.Find("Tires").transform.Find("Chev_LF");

        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftCollider, frontLeftWheel);
        UpdateWheelPose(frontRightCollider, frontRightWheel);
        UpdateWheelPose(rearLeftCollider, rearLeftWheel);
        UpdateWheelPose(rearRightCollider, rearRightWheel);
    }

    private void UpdateWheelPose(WheelCollider collider, Transform wheel)
    {
        Vector3 position = wheel.position;
        Quaternion quat = wheel.rotation;

        collider.GetWorldPose(out position, out quat);
    }

    private void Accelerate()
    {
        frontLeftCollider.motorTorque = verticalInput * motorForce;
        frontRightCollider.motorTorque = verticalInput* motorForce;
    }

    private void Steer()
    {
        steeringAngle = maxSteeringAngle * horizontalInput;
        frontLeftCollider.steerAngle = steeringAngle;
        frontRightCollider.steerAngle = steeringAngle;


    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    void shootProjectile()
    {
        Debug.Log("Called 1");
        if (shootTimer > 0.3){
            Debug.Log("Called 2");
            Instantiate(mainMissile, cars[playerIndex].transform.position, cars[playerIndex].transform.rotation);
            shootTimer = 0;
        }
    }
}
