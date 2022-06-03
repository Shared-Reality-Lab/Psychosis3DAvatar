using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartEnd : MonoBehaviour
{
    // Start is called before the first frame update
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
    public void ButtonAvatarPress()
    {
        refernce.PressButtonSend("ATAvatar");
        SceneManager.LoadScene("AT_Avatar", LoadSceneMode.Single);
    }
    public void ButtonTherapistPress()
    {
        refernce.PressButtonSend("ATTherapist");
        SceneManager.LoadScene("AT_Therapist_latest", LoadSceneMode.Single);//AT_Therapist or button option
    }
    
}
