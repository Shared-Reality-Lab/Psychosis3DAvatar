using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PatientStatus : MonoBehaviour
{
    public Text show;
    void Start()
    {
        show.text="Please wait ...";
        PlayerPrefs.SetString("Clientreceive","initialize");
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetString("Clientreceive")=="IsReady")
        {
            show.text="User is Ready";
        }
        //Debug.Log(PlayerPrefs.GetString("Clientreceive"));
    }
}
