using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMissile : MonoBehaviour
{
    Rigidbody rb;
    GameObject carOne;
    GameObject carTwo;
    CarMovementScript carScript;
    bool hitFloor;
    public int missileSender;
    float lifeSpan;
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();
        Debug.Log("Created");
        //carOne = GameObject.Find("Car1");
        //carTwo = GameObject.Find("Car2");
        lifeSpan = 0;
        hitFloor = false;
        //transform.Rotate(0, 90, 90);
        string missileName = transform.name;
        carScript = (CarMovementScript)GameObject.Find("NetworkManager").GetComponent(typeof(CarMovementScript));
        transform.rotation = carScript.getRotation();
        Debug.Log("missilenum " + missileSender);
        Debug.Log("playerIndex " + carScript.getPlayerIndex());
    }

    // Update is called once per frame
    void Update(){
        lifeSpan += Time.deltaTime;
        if (lifeSpan >= 0.5){
            rb.AddForce(30 * carScript.missileForce * transform.up, ForceMode.Acceleration);
        }
        else {
            rb.AddForce(30 * carScript.missileForce * transform.forward, ForceMode.Acceleration);
        }
        if (lifeSpan >= 4) {
            Destroy(this.gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide Name: " + collision.gameObject.name);
        if (missileSender == 0)
        {
            Debug.Log("Called 1");
            if (collision.gameObject.name.Contains("Car2") )
//&& carScript.getPlayerIndex() == 1
            {
                Debug.Log("Called 2");
                carScript.ReducePlayerHealth(1);
                Debug.Log("Called 3");
                DestroyFunction();
            }
            else if (!collision.gameObject.name.Contains("Car1") && !collision.gameObject.name.Contains("Plane"))
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Debug.Log("Called 4");
            if (collision.gameObject.name.Contains("Car1") )
//&& carScript.getPlayerIndex() == 0
            {
                Debug.Log("Called 5");
                carScript.ReducePlayerHealth(0);
                Debug.Log("Called 6");
                DestroyFunction();
            }
            else if (!collision.gameObject.name.Contains("Car2") && !collision.gameObject.name.Contains("Plane"))
            {
                Destroy(this.gameObject);
            }
        }
        //if (!collision.gameObject.name.Contains("Car") && !collision.gameObject.name.Contains("Plane"))
        //{
        //    Destroy(this.gameObject);
        //}
    }

    public void DestroyFunction(){
        Destroy(this.gameObject);
    }
}
