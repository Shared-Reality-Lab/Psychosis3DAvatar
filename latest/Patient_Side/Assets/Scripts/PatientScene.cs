using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientScene : MonoBehaviour
{
    string P_therapyType;
    string P_server;
    string P_CpuGpu;
    string P_SV2RAI;
    void Start()
    {
        P_therapyType= PlayerPrefs.GetString("therapyType");
        P_server = PlayerPrefs.GetString("server");
        P_CpuGpu = PlayerPrefs.GetString("CpuGpu");
        P_SV2RAI = PlayerPrefs.GetString("SV2RAI");

        Debug.Log("Enter patient scene");
        Debug.Log("therapyType:"+P_therapyType+" server:"+ P_server+ " CpuGpu:"+ P_CpuGpu+ " SV2orRAI:"+ P_SV2RAI);
        if (P_server == "Unicon")//Add connection here
        {
            Debug.Log("AAAAAA");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
