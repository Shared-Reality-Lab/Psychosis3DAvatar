using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    GameObject hide;
    
    void Start()
    {
        hide=GameObject.Find("Startclient");
        //refernce=(CustomServer)getClient.GetComponent(typeof(CustomServer));
        hide.transform.position=new Vector3(2,-100,0);
    }
}
