using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.UI;
using TMPro;

public class CarMovementScript : MonoBehaviourPunCallbacks
{
    float horizontalInput;
    float verticalInput;
    GameObject[] cars;
    GameObject[] missileArray;
    public GameObject carOne;
    public GameObject carTwo;
    int playerIndex;
    public Camera mainCamera;
    public Camera cameraOne;
    public Camera cameraTwo;
    public GameObject missileOne;
    public GameObject missileTwo;
    public TextMeshPro healthDisplay;
    public TextMeshPro rocketDisplay;
    public TextMeshPro winOrLossDisplay;

    GameObject mainMissile;
    GameObject enemyMissile;
    public Canvas canvas;

    int myHealth = 3;
    int enemyHealth = 3;

    WheelCollider frontLeftCollider;
    WheelCollider frontRightCollider;
    WheelCollider rearLeftCollider;
    WheelCollider rearRightCollider;

    Transform frontLeftWheel;
    Transform frontRightWheel;
    Transform rearLeftWheel;
    Transform rearRightWheel;

    public float motorForce;
    public float brakeForce;
    float shootTimer;
    float maxHealth = 6;
    public float maxSteeringAngle;

    bool lost;
    bool enemyLost;

    public float missileForce;

    float steeringAngle;

    public static CarMovementScript instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
    }

    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        cars = new GameObject[2];
        cars[0] = carOne;
        cars[1] = carTwo;

        shootTimer = 0;

        lost = false;
        enemyLost = false;

        Rigidbody rb1 = carOne.GetComponent<Rigidbody>();
        Rigidbody rb2 = carTwo.GetComponent<Rigidbody>();
        rb1.centerOfMass = new Vector3(rb1.centerOfMass.x, rb1.centerOfMass.y - 0.75f, rb1.centerOfMass.z);
        rb2.centerOfMass = new Vector3(rb2.centerOfMass.x, rb2.centerOfMass.y - 0.75f, rb2.centerOfMass.z);


        int count = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.Equals(PhotonNetwork.LocalPlayer))
            {
                playerIndex = count;
            }
            count++;
        }

        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        //mainCamera.enabled = false;
        if (playerIndex == 0)
        {
            cameraTwo.enabled = false;
            mainMissile = missileOne;
            enemyMissile = missileTwo;
            canvas.worldCamera = cameraOne;
        }
        else
        {
            cameraOne.enabled = false;
            mainMissile = missileTwo;
            enemyMissile = missileOne;
            canvas.worldCamera = cameraTwo;
        }
        missileForce = 10;

        ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
        setValue.Add("p1Health", 3);
        setValue.Add("p2Health", 3);
        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);

    }

    void Update()
    {
        if (!lost && !enemyLost)
        {
            //PhotonNetwork.LocalPlayer.CustomProperties.Clear();
            ExitGames.Client.Photon.Hashtable propertyHash = new ExitGames.Client.Photon.Hashtable();
            propertyHash.Add("Position", cars[playerIndex].transform.position);
            propertyHash.Add("Rotation", cars[playerIndex].transform.rotation);

            GameObject[] otherMissiles;

            if (playerIndex == 0) {
                missileArray = GameObject.FindGameObjectsWithTag("BlueMissile");
                otherMissiles = GameObject.FindGameObjectsWithTag("RedMissile");
            }
            else
            {
                missileArray = GameObject.FindGameObjectsWithTag("RedMissile");
                otherMissiles = GameObject.FindGameObjectsWithTag("BlueMissile");
            }

            Vector3[] missilePositionArray = new Vector3[missileArray.Length];
            Quaternion[] missileRotationArray = new Quaternion[missileArray.Length];

            for (int i = 0; i < missileArray.Length; i++)
            {
                missilePositionArray[i] = missileArray[i].transform.position;
                missileRotationArray[i] = Quaternion.Euler(cars[playerIndex].transform.forward);

            }


            propertyHash.Add("MissilePositions", missilePositionArray);
            propertyHash.Add("MissileRotations", missileRotationArray);
            Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties.ToString());
            int health;
            int p1Health = (int)PhotonNetwork.CurrentRoom.CustomProperties["p1Health"];
            int p2Health = (int)PhotonNetwork.CurrentRoom.CustomProperties["p2Health"];
            if (playerIndex == 0)
            {
                health = p1Health;
            }
            else{
                health = p2Health;
            }
            string text = "";
            for (int i = 0; i < health; i++)
            {
                text += "<sprite index= 0>  ";
            }
            healthDisplay.text = text;

            if (health <= 0)
            {
                lost = true;
            }

            propertyHash.Add("LostGame", lost);
            PhotonNetwork.LocalPlayer.SetCustomProperties(propertyHash);

            for (int i = otherMissiles.Length - 1; i >= 0; i--)
            {
                GameObject.Destroy(otherMissiles[i].gameObject);
            }

            shootTimer += Time.deltaTime;

            int count = 0;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.Equals(PhotonNetwork.LocalPlayer) && player.CustomProperties["Position"] != null)
                {
                    cars[count].transform.position = (Vector3)player.CustomProperties["Position"];
                    cars[count].transform.rotation = (Quaternion)player.CustomProperties["Rotation"];

                    enemyLost = (bool)player.CustomProperties["LostGame"];
                    Vector3[] enemyMissilePositions = (Vector3[])player.CustomProperties["MissilePositions"];
                    Quaternion[] enemyMissileRotations = (Quaternion[])player.CustomProperties["MissileRotations"];

                    Quaternion enemyRotation;
                    if(playerIndex == 0)  {
                        enemyRotation = Quaternion.Euler(cars[1].transform.forward);
                    }
                    else
                    {
                        enemyRotation = Quaternion.Euler(cars[0].transform.forward);
                    }
                    for (int i = 0; i < enemyMissilePositions.Length; i++)
                    {
                        GameObject.Instantiate(enemyMissile, enemyMissilePositions[i], Quaternion.identity);
                    }
                }


                count++;
            }
        }
        else
        {
            if (lost)
            {
                //Display Text that says you lost
                Time.timeScale = 0;
                winOrLossDisplay.text = "You lose!";
                healthDisplay.text = "";
            }
            else
            { // enemy lost
                Time.timeScale = 0;
                //Display Text that says you won
                winOrLossDisplay.text = "You win!";
            }
        }
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        applyMovement(cars[playerIndex]);

        if (Input.GetKeyDown(KeyCode.Space)) {
            shootProjectile();
        }

    }
    void applyMovement(GameObject car)
    {
        car.GetComponent<Rigidbody>().AddForce(0, -10, 0);
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
        Brake();

    }
    private void Brake()
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            //Debug.Log("Brake force: " + brakeForce);   
            rearLeftCollider.brakeTorque = Mathf.Clamp(Mathf.Abs(rearLeftCollider.rpm) * 8f, 100, 10000);
            rearRightCollider.brakeTorque = Mathf.Clamp(Mathf.Abs(rearRightCollider.rpm) * 8f, 100, 10000);
            frontLeftCollider.brakeTorque = Mathf.Clamp(Mathf.Abs(frontLeftCollider.rpm) * 8f, 100, 10000);
            frontRightCollider.brakeTorque = Mathf.Clamp(Mathf.Abs(frontRightCollider.rpm) * 8f, 100, 10000);

            //Debug.Log(frontLeftCollider.brakeTorque);
        }
        else
        {
            rearLeftCollider.brakeTorque = 0;
            rearRightCollider.brakeTorque = 0;
            frontLeftCollider.brakeTorque = 0;
            frontRightCollider.brakeTorque = 0;
        }
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
        frontRightCollider.motorTorque = verticalInput * motorForce;
    }

    private void Steer()
    {
        steeringAngle = maxSteeringAngle * (horizontalInput * 0.6f);
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
        Debug.Log("Shot");
        if (shootTimer > 0.3)
        {
            Vector3 origPosition = cars[playerIndex].transform.Find("MissilePoint").transform.position;
            Vector3 missilePosition = new Vector3(origPosition.x, origPosition.y, origPosition.z);

            Vector3 origRotation = Quaternion.ToEulerAngles(cars[playerIndex].transform.Find("MissilePoint").transform.rotation);
            Vector3 missileRotation = new Vector3(origRotation.x + 90, origRotation.y + 90, origRotation.z + 90);
            GameObject.Instantiate(mainMissile, missilePosition, Quaternion.Euler(cars[playerIndex].transform.forward));
            shootTimer = 0;
        }
    }
    public void IncreaseHealth()
    {
        int p1Health = (int)PhotonNetwork.CurrentRoom.CustomProperties["p1Health"];
        int p2Health = (int)PhotonNetwork.CurrentRoom.CustomProperties["p2Health"];
        int health;
        if (playerIndex == 0 && p1Health < maxHealth)
        {
            p1Health++;
            healthDisplay.text += "<sprite index= 0>  ";
        }
        else if (playerIndex == 1 && p2Health < maxHealth)
        {
            p2Health++;
            healthDisplay.text += "<sprite index= 0>  ";
        }

        ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
        setValue.Add("p1Health", p1Health);
        setValue.Add("p2Health", p2Health);

        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);
      

    }

    public int getPlayerIndex()
    {
        return playerIndex;
    }
    public Quaternion getRotation()
    {
        return cars[playerIndex].transform.rotation;
    }
    public void IncreaseMissileRange()
    {
        missileForce += 2;
        rocketDisplay.text = "Rocket Boost : " + (missileForce - 10);

    }

    public void ReducePlayerHealth(int player)
    {
        int p1Health = (int)PhotonNetwork.CurrentRoom.CustomProperties["p1Health"];
        int p2Health = (int)PhotonNetwork.CurrentRoom.CustomProperties["p2Health"];

        ExitGames.Client.Photon.Hashtable setValue = new ExitGames.Client.Photon.Hashtable();
        if (player == 0)
        {
            setValue.Add("p1Health", p1Health - 1);
            setValue.Add("p2Health", p2Health);
        }
        else
        {
            setValue.Add("p1Health", p1Health);
            setValue.Add("p2Health", p2Health - 1);
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);

        Debug.Log("Reduce Health Called");
        string text = "";
        //myHealth--;
        int health;
        if (playerIndex == 0) {
            health = p1Health;
        }
        else {
            health = p2Health;
        }
        for (int i = 0; i < health; i++)
        {
            text += "<sprite index= 0>  ";
        }
        healthDisplay.text = text;

    }
}
