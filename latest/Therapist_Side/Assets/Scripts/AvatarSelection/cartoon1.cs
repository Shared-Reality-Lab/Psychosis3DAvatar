using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class cartoon1 : MonoBehaviour
{
    GameObject getClient;
    CustomClient refernce;
    public Dropdown Voice;
    void Start()
    {
        getClient = GameObject.Find("Client");
        refernce = (CustomClient)getClient.GetComponent(typeof(CustomClient));

    }
    public void ButtonPress()
    {
        refernce.PressButtonSend("ATAvatar");
        if (Voice.options[Voice.value].text == "Male")
        {
            PlayerPrefs.SetString("Voice", "Male");

        }
        else
        {
            PlayerPrefs.SetString("Voice", "Female");

        }

        SceneManager.LoadScene("AT_Avatar", LoadSceneMode.Single);
    }
}
