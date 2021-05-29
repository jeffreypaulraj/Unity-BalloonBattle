using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMissile : MonoBehaviour
{
    Rigidbody rb;
    GameObject carOne;
    GameObject carTwo;
    CarMovementScript carScript;
    float missileForce = 10;
    public int missileSender;
    float lifeSpan;
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.AddForce(missileForce * transform.forward, ForceMode.Acceleration);
        carOne = GameObject.Find("Car1");
        carTwo = GameObject.Find("Car2");
        lifeSpan = 0;
        string missileName = transform.name;
        carScript = (CarMovementScript)GameObject.Find("NetworkManager").GetComponent(typeof(CarMovementScript));
        if (name.Contains("Blue")){
            missileSender = 0;
        }
        else {
            missileSender = 1;
        }
    }

    // Update is called once per frame
    void Update(){
        lifeSpan += Time.deltaTime;
        if(lifeSpan >= 4) {
            Destroy(this.gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision){
        //Debug.Log(collision.gameObject.name);
        if(missileSender == 0){
            if(collision.gameObject == carTwo){
                carScript.ReduceHealth();
                Destroy(this.gameObject);
            }
            else if(collision.gameObject != carOne && !collision.gameObject.name.Contains("Plane")){
                //Destroy(this.gameObject);
            }
        }
        else {
            if (collision.gameObject == carOne){
                carScript.ReduceHealth();
                Destroy(this.gameObject);
            }
            else if (collision.gameObject != carTwo && !collision.gameObject.name.Contains("Plane")){
                //Destroy(this.gameObject);
            }
        }
    }
    public void IncreaseMissileRange()
    {
        missileForce *= 1.2f;
    }
}
