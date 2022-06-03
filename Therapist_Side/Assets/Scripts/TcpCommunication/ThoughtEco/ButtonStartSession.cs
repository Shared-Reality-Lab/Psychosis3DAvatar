using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStartSession : MonoBehaviour
{
    GameObject getClient;
    CustomClient refernce;
    void Start()
    {
        getClient=GameObject.Find("Client");
        refernce=(CustomClient)getClient.GetComponent(typeof(CustomClient));
        
    }
    public void ButtonPress()
    {
        refernce.PressButtonSend("StartSession");
    }
    public void ButtonEndPress()
    {
        refernce.PressButtonSend("EndSession");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
