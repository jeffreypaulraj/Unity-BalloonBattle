using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMysteryBoxes : MonoBehaviour
{
    public GameObject mysteryBox;
    List<Vector3> locations = new List<Vector3>();
    public float respawnTime;
    

    // Start is called before the first frame update
    void Start()
    {
        locations = new List<Vector3>(){
                    new Vector3(11.4f, 0, -147),
                    new Vector3(11.4f, 0, 147),
                    new Vector3(-148, 0, -0.1f),
                    new Vector3(188, 0, -0.1f),
                    new Vector3(11.4f , 35, -0.1f),
                    };
        foreach(Vector3 location in locations) {
            GameObject newBox = Instantiate(mysteryBox);
            newBox.transform.position = location;
        }
    }

    void Update(){
 
    }
}
