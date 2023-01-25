using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Av_Record : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        
        //foreach (var device in Microphone.devices)
        //{
        //    Debug.Log("Name: " + device);
        //}
    }

    public void PressStart()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, false, 50, 44100);

        
    }
    public void PressPlay()
    {
        
        audioSource.Play();
    }
    public void SaveAudio()
    {
        SavWav.Save("D:/SRL/audio/129", audioSource.clip);
    }
    //F:/SRL/SimpleTryOut
    // Update is called once per frame
    void Update()
    {
        
    }
}
