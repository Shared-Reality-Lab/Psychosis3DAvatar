using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TherapistMode : MonoBehaviour
{
    public GameObject therapistUI;
    GameObject getClient;
    CustomClient refernce;
    void Start()
    {
        getClient=GameObject.Find("Client");
        refernce=(CustomClient)getClient.GetComponent(typeof(CustomClient));
        
    }
    public void OnButtonClicked(){
        therapistUI.SetActive(true);
        refernce.StartGetInfo();
    }
}
