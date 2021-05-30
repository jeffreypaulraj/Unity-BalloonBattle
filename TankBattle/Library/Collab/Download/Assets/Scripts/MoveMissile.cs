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
    bool hitFloor;
    public int missileSender;
    float lifeSpan;
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();
        carOne = GameObject.Find("Car1");
        carTwo = GameObject.Find("Car2");
        lifeSpan = 0;
        hitFloor = false;
        //transform.Rotate(0, 90, 90);
        string missileName = transform.name;
        carScript = (CarMovementScript)GameObject.Find("NetworkManager").GetComponent(typeof(CarMovementScript));
        transform.rotation = carScript.getRotation();
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
        if (lifeSpan >= 0.5){
            rb.AddForce(30 * missileForce * transform.up, ForceMode.Acceleration);
        }
        else {
            rb.AddForce(30 * missileForce * transform.forward, ForceMode.Acceleration);
        }
        //rb.AddForce(-5 * transform.forward, ForceMode.Acceleration);
        if (lifeSpan >= 4) {
            Destroy(this.gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name.Contains("Plane")){
            hitFloor = true;
        }
        Debug.Log(collision.gameObject.name);
        if (missileSender == 0){
            if(collision.gameObject == carTwo){
                carScript.ReduceHealth();
                Destroy(this.gameObject);
            }
            else if(collision.gameObject != carOne && !collision.gameObject.name.Contains("Plane")){
                Destroy(this.gameObject);
            }
        }
        else {
            if (collision.gameObject == carOne){
                carScript.ReduceHealth();
                Destroy(this.gameObject);
            }
            else if (collision.gameObject != carTwo && !collision.gameObject.name.Contains("Plane")){
                Destroy(this.gameObject);
            }
        }
    }
    public void IncreaseMissileRange()
    {

        missileForce *= 1.2f;
    }
}
