using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Switch Scene to 
/// </summary>
/// 
public class ChangeSelection : MonoBehaviour
{
    //public Dropdown therapyType;
    //public Dropdown server;
    GameObject getClient;
    CustomClient refernce;
    void Start()
    {
        getClient = GameObject.Find("Client");
        refernce = (CustomClient)getClient.GetComponent(typeof(CustomClient));

    }
    public void ButtonPressed()
    {
        //if(therapyType.options[therapyType.value].text=="Avatar Therapy")
        //{
        //    if(server.options[server.value].text=="Unicorn")
        //    {
        //        refernce.PressButtonSend("ATUnicorn");
        //        PlayerPrefs.SetString("Server","Unicorn");
        //    }
        //    if(server.options[server.value].text=="Pegasus")
        //    {
        //        refernce.PressButtonSend("ATPegasus");
        //        PlayerPrefs.SetString("Server","Pegasus");
        //    }

        //    //refernce.PressButtonSend("AvatarTherapy");
        //    SceneManager.LoadScene("AvatarTherapy", LoadSceneMode.Single);
        //}

        refernce.PressButtonSend("ATAvatar");
        SceneManager.LoadScene("AT_Avatar", LoadSceneMode.Single);

    }
}
