using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class record : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audioSource;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PressStart()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, false, 5, 44100);

    }
    public void PressPlay()
    {
        
        audioSource.Play();
    }
    public void SaveAudio()
    {
        SavWav.Save("C:/Users/vishav3/Desktop/recordedWav/send", audioSource.clip);
    }
}
