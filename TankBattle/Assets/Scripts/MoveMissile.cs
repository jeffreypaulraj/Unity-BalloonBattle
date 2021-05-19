using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMissile : MonoBehaviour
{
    public int playerIndex;
    Rigidbody rb;
    GameObject carOne;
    GameObject carTwo;
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.AddForce(10 * transform.forward, ForceMode.Acceleration);
        carOne = GameObject.Find("Car1");
        carTwo = GameObject.Find("Car2");
    }

    // Update is called once per frame
    void Update(){
        
    }

    private void OnCollisionEnter(Collision collision){
        Debug.Log(collision.gameObject.name);
        if(playerIndex == 0 && collision.gameObject != carOne){
            //Destroy(this.gameObject);
        }
        else if(playerIndex == 1 && collision.gameObject != carTwo){
            //Destroy(this.gameObject);
        }
    }
}
