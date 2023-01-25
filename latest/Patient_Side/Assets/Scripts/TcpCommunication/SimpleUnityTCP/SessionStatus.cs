using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SessionStatus : MonoBehaviour
{
    public Text show;

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetString("Serverreceive")=="StartSession")
        {
            show.text="";
        }

        if(PlayerPrefs.GetString("Serverreceive")=="EndSession")
        {
            show.text="Your session is end";
            SceneManager.LoadScene("End", LoadSceneMode.Single);
        }
    }
}
