using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StatusAvatar : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        
        if(PlayerPrefs.GetString("Serverreceive")=="ATTherapist")
        {
            Debug.Log("ATTherapist");
            SceneManager.LoadScene("AT_therapist", LoadSceneMode.Single);
        }
        if(PlayerPrefs.GetString("Serverreceive")=="EndSession")
        {
            Debug.Log("EndSession");
            SceneManager.LoadScene("End", LoadSceneMode.Single);
        }
    }
}
