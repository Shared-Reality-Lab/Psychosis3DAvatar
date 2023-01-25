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
        ////Thought echo with diff server
                
        //load avatar therapy scene
        if(PlayerPrefs.GetString("Serverreceive")=="ATAvatar")
        {
            
            Debug.Log("ATF_cartoon");
            SceneManager.LoadScene("ATF", LoadSceneMode.Single);
        }
        //if(PlayerPrefs.GetString("Serverreceive")=="ATTherapist")
        //{
        //    Debug.Log("ATF_real");
        //    SceneManager.LoadScene("ATF_real", LoadSceneMode.Single);
        //}
    }
}
