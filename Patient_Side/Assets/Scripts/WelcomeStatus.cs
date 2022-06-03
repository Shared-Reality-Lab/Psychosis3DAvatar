using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WelcomeStatus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("Serverreceive","PatientSide");
    }

    // Update is called once per frame
    void Update()
    {
        //Thought echo with diff server
        if(PlayerPrefs.GetString("Serverreceive")=="TEUnicorn")
        {
            Debug.Log("TEUnicorn");
            PlayerPrefs.SetString("Server","Unicorn");
            SceneManager.LoadScene("TE4", LoadSceneMode.Single);
        }

        if(PlayerPrefs.GetString("Serverreceive")=="TEPegasus")
        {
            Debug.Log("Thought Echo");
            PlayerPrefs.SetString("Server","Pegasus");
            SceneManager.LoadScene("TE4", LoadSceneMode.Single);
        }

        //Avatar therapy with diff server
        if(PlayerPrefs.GetString("Serverreceive")=="ATUnicorn")
        {
            Debug.Log("ATUnicorn");
            PlayerPrefs.SetString("Server","Unicorn");
        }
        if(PlayerPrefs.GetString("Serverreceive")=="ATPegasus")
        {
            Debug.Log("ATPegasus");
            PlayerPrefs.SetString("Server","Pegasus");
        }
        
        //load avatar therapy scene
        if(PlayerPrefs.GetString("Serverreceive")=="ATAvatar")
        {
            
            Debug.Log("ATAvatar");
            SceneManager.LoadScene("ATF", LoadSceneMode.Single);
        }
        if(PlayerPrefs.GetString("Serverreceive")=="ATTherapist")
        {
            Debug.Log("ATTherapist");
            SceneManager.LoadScene("ATT", LoadSceneMode.Single);
        }
    }
}
