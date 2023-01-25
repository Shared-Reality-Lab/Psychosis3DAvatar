using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Switch2Therapist : MonoBehaviour
{
    GameObject getClient;
    CustomClient refernce;
    void Start()
    {
        getClient=GameObject.Find("Client");
        refernce=(CustomClient)getClient.GetComponent(typeof(CustomClient));
        
    }
    public void Switch2The()
    {
        refernce.PressButtonSend("ATTherapist");
        SceneManager.LoadScene("AT_Therapist_latest", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
