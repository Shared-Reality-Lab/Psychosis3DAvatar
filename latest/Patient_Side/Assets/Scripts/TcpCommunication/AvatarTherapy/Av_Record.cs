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
        Debug.Log("StartRecording");
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, false, 300, 44100);
        
    }
    public void PressPlay()
    {
        
        audioSource.Play();
    }
    public void SaveAudio()
    {
        
        
        
        
        
        SavWav.Save("C:/Users/vishav3/Desktop/recordedWav/1111", audioSource.clip);
    }


    public void StartRecording()
    {
        

    }

    //IEnumerator EndRecording(AudioSource audS, string deviceName)
    //{

    //    Debug.Log("EndRecording");
    //    //other.StartNewThread();
    //    timer = 0.0f;
    //    recording = false;
    //    //Capture the current clip data
    //    AudioClip recordedClip = audS.clip;
    //    var position = Microphone.GetPosition(deviceName);
    //    var soundData = new float[recordedClip.samples * recordedClip.channels];
    //    recordedClip.GetData(soundData, 0);

    //    //Create shortened array for the data that was used for recording
    //    var newData = new float[position * recordedClip.channels];

    //    //$$anonymous$$icrophone.End (null);
    //    //Copy the used samples to a new array
    //    for (int i = 0; i < newData.Length; i++)
    //    {
    //        newData[i] = soundData[i];
    //    }

    //    // One does not simply shorten an AudioClip,
    //    // so we make a new one with the appropriate length
    //    var newClip = AudioClip.Create(recordedClip.name, position, recordedClip.channels, recordedClip.frequency, false);
    //    newClip.SetData(newData, 0);        // Give it the data from the old clip

    //    // Replace the old clip
    //    AudioClip.Destroy(recordedClip);
    //    audS.clip = newClip;
    //    saveCount++;
    //    string filename = "D:/SRL/audio/patient.wav";
    //    //save and send
    //    SavWav.Save(filename, CurrentAudioSource.clip);
    //    SendWav();
    //    yield return null;

    //}


    void Update()
    {
        
    }
}
