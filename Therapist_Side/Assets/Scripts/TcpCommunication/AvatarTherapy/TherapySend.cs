using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TherapySend : MonoBehaviour
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
        refernce.SendWav();
    }
}
