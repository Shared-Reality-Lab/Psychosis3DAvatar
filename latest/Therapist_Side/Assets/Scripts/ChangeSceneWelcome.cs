using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeSceneWelcome : MonoBehaviour
{
    public CustomClient accessCustomServer;

    public void ButtonTwoPressed()
    {
        //base.SendMessageToClient("IsReady");
        accessCustomServer.PressButtonSend("IsReady");
        SceneManager.LoadScene("Patient", LoadSceneMode.Single);
        
    }


    
}