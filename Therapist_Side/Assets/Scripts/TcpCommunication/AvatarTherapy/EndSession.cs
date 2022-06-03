using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSession : MonoBehaviour
{
    GameObject getClient;
    CustomClient refernce;
    void Start()
    {
        getClient=GameObject.Find("Client");
        refernce=(CustomClient)getClient.GetComponent(typeof(CustomClient));
        
    }
    public void ButtonEndPress()
    {
        refernce.PressButtonSend("EndSession");
    }
}
