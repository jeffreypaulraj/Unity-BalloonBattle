using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBoxCollisionManager : MonoBehaviour
{
    bool isActive;
    public GameObject networkManager;
    CarMovementScript carMovementScript;
    GameObject box;
    float timer;
    float maxTime = 15;

    // Start is called before the first frame update
    void Start()
    {
        box = GameObject.Find("Box001");
        timer = 0;
        isActive = true;
        carMovementScript = (CarMovementScript)networkManager.GetComponent(typeof(CarMovementScript));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(!isActive && timer > maxTime)
        {
            box.GetComponent<MeshRenderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
            isActive = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("On trigger entered");
        if (isActive && other.gameObject.CompareTag("Car"))
        {
            GivePowerUp();
            box.GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            isActive = false;
            timer = 0;
        }
    }
    public void GivePowerUp(){
        System.Random rand = new System.Random();
        var n = rand.Next(0, 2);

        if(n == 0) {
            carMovementScript.IncreaseHealth();
            
        }
        else{ // n == 1
            //moveMissile.IncreaseMissileRange();
        }
        
    }
}
