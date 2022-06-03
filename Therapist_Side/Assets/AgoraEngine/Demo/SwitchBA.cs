using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.SceneManagement;
public class SwitchBA : MonoBehaviour
{
    public void OnPress()
    {
        SceneManager.LoadScene (sceneName:"try",LoadSceneMode.Single);
    }
}
