using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementScript : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    bool breaking;


    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        breaking = Input.GetKey(KeyCode.Space);

        Rigidbody rb = this.gameObject.transform.GetComponent<Rigidbody>();
        


    }
}
