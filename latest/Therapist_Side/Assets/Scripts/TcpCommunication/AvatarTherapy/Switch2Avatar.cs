using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Switch2Avatar : MonoBehaviour
{
    GameObject getClient;
    CustomClient refernce;
    void Start()
    {
        getClient=GameObject.Find("Client");
        refernce=(CustomClient)getClient.GetComponent(typeof(CustomClient));
        
    }
    public void Switch2Ava()
    {
        refernce.PressButtonSend("ATAvatar");
        SceneManager.LoadScene("AT_Avatar", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
