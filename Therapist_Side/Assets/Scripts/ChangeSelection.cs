using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ChangeSelection : MonoBehaviour
{
    public Dropdown therapyType;
    public Dropdown server;
    GameObject getClient;
    CustomClient refernce;
    void Start()
    {
        getClient=GameObject.Find("Client");
        refernce=(CustomClient)getClient.GetComponent(typeof(CustomClient));
        
    }
    public void ButtonPressed()
    {
        if(therapyType.options[therapyType.value].text=="Avatar Therapy")
        {
            if(server.options[server.value].text=="Unicorn")
            {
                refernce.PressButtonSend("ATUnicorn");
                PlayerPrefs.SetString("Server","Unicorn");
            }
            if(server.options[server.value].text=="Pegasus")
            {
                refernce.PressButtonSend("ATPegasus");
                PlayerPrefs.SetString("Server","Pegasus");
            }
          
            //refernce.PressButtonSend("AvatarTherapy");
            SceneManager.LoadScene("AvatarTherapy", LoadSceneMode.Single);
        }

        if(therapyType.options[therapyType.value].text=="Thought Echo")
        {
            if(server.options[server.value].text=="Unicorn")
            {
                refernce.PressButtonSend("TEUnicorn");
                PlayerPrefs.SetString("Server","Unicorn");
            }
            if(server.options[server.value].text=="Pegasus")
            {
                refernce.PressButtonSend("TEPegasus");
                PlayerPrefs.SetString("Server","Pegasus");
            }
            //refernce.PressButtonSend("ThoughtEcho");
            SceneManager.LoadScene("ThoughtEco", LoadSceneMode.Single);
        }
        
        
    }
}
