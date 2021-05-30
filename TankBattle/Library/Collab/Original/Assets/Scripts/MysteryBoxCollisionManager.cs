using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBoxCollisionManager : MonoBehaviour
{
    bool isActive;
    public GameObject networkManager;
    CarMovementScript carMovementScript;
    MoveMissile moveMissileScript;
    public GameObject box;
    float timer;
    float maxTime = 10;

    // Start is called before the first frame update
    void Start()
    {
        
        timer = 0;
        isActive = true;
        networkManager = GameObject.Find("NetworkManager");
        carMovementScript = networkManager.GetComponent<CarMovementScript>();//(CarMovementScript)networkManager.GetComponent(typeof(CarMovementScript));
        
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(!isActive && timer > maxTime){
            //Debug.Log("reenabling box");
            GetComponent<BoxCollider>().enabled = true;

            box.SetActive(true);
            box.GetComponent<MeshRenderer>().enabled = true;

            isActive = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (isActive && other.gameObject.CompareTag("Car")){
            GivePowerUp();

            box.GetComponent<MeshRenderer>().enabled = false;
            box.SetActive(false);
            

            GetComponent<BoxCollider>().enabled = false;

            isActive = false;
            timer = 0;

            //Debug.Log("Box renderer: " + box.GetComponent<MeshRenderer>().enabled);
        }
    }
    public void GivePowerUp(){
        System.Random rand = new System.Random();
        var n = rand.Next(0, 2);

        if(n == 0) {
            carMovementScript.IncreaseHealth();
            
        }
        else{ // n == 1
            //networkManager.GetComponent<CarMovementScript>().RocketPowerUp(true);
            carMovementScript.IncreaseMissileRange();
        }
        
    }
}
