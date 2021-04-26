using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{
    public Button joinCreateButton;
    public InputField codeInputField;
    public NetworkManager networkManager;
    public NetworkManagerScript networkManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        joinCreateButton.onClick.AddListener(JoinCreateRoom);
        networkManager = FindObjectOfType<NetworkManager>();
        networkManagerScript = (NetworkManagerScript)networkManager.GetComponent(typeof(NetworkManagerScript));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void JoinCreateRoom()
    {
        string code = codeInputField.text;
        networkManagerScript.JoinOrCreateRoom(code);
        SceneManager.LoadScene("Game");

    }
}
