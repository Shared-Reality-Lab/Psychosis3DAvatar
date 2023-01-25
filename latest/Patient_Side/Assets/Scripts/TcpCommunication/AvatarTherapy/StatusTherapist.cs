using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StatusTherapist : MonoBehaviour
{
    public GameObject  reference;
    public HelloUnity3D script;

    void Start()
    {
        script = reference.GetComponent<HelloUnity3D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetString("Serverreceive")=="ATAvatar")
        {
            script.LeaveChannel();
            Debug.Log("ATAvatar");
            SceneManager.LoadScene("ATF", LoadSceneMode.Single);
        }
        if(PlayerPrefs.GetString("Serverreceive")=="EndSession")
        {
            script.LeaveChannel();
            Debug.Log("EndSession");
            SceneManager.LoadScene("End", LoadSceneMode.Single);
        }
    }
}
