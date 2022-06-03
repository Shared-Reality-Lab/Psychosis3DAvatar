using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StoreValue : MonoBehaviour
{
    // Start is called before the first frame update
    public Dropdown therapyType;
    public Dropdown server;
    public Dropdown CpuGpu;
    public Dropdown SV2RAI;
    // Update is called once per frame

    public void ButtonOnePressed()
    {
        PlayerPrefs.SetString("therapyType", therapyType.options[therapyType.value].text);
        PlayerPrefs.SetString("server", server.options[server.value].text);
        PlayerPrefs.SetString("CpuGpu", CpuGpu.options[CpuGpu.value].text);
        PlayerPrefs.SetString("SV2RAI", SV2RAI.options[SV2RAI.value].text);

        SceneManager.LoadScene("Patient", LoadSceneMode.Single);
        
    }

}
