using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStop : MonoBehaviour
{
    public int pauselength;
    AudioSource CurrentAudioSource;
    AudioClip audio_;
    bool recording=false;
    int sample_size = 128;
    float timer = 0.0f;
    float seconds;
    int saveCount = 0;
    void Start()
    {
        CurrentAudioSource = GetComponent<AudioSource>();
        audio_ = CurrentAudioSource.clip;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        if (recording)
        {
            float[] spectrum = new float[sample_size];
            int position2 = Microphone.GetPosition(null) - (sample_size + 1);
            if (position2 < 0)
            {
                return;
            }

            CurrentAudioSource.clip.GetData(spectrum, position2);

            for (int i = 0; i < spectrum.Length; i++)
            {
                float peak = spectrum[i] * spectrum[i];
                /*Debug.Log("Peak");
                Debug.Log(peak);*/
                if (peak > 5E-08)
                {
                    //level_ = peak;
                    timer = 0.0f;
                    Debug.Log("You are speaking");
                }

                else
                {
                    Debug.Log("You are not speaking");
                    Debug.Log("For");
                    timer += Time.deltaTime;
                    seconds = timer;//(int)(timer % 60);
                    Debug.Log(seconds);
                    Debug.Log("mseconds");
                    if(seconds> pauselength)
                    {
                        Debug.Log("Pause! EndRecording");
                        EndRecording(CurrentAudioSource, "");

                        
                    }



                }
            }
        }
        
    }
    public void ButtonStart()
    {
        StartRecording();
        
    }

    public void ButtonStop()
    {
        EndRecording(CurrentAudioSource, "");
        
    }

    public void PressPlay()
    {

        CurrentAudioSource.Play();
    }
    
    void StartRecording()
    {
        timer = 0.0f;
        CurrentAudioSource.clip = Microphone.Start(null, false, 300, 44100);
        recording = true;
        
    }
    void EndRecording(AudioSource audS, string deviceName)
    {
        timer = 0.0f;
        recording = false;
        //Capture the current clip data
        AudioClip recordedClip = audS.clip;
        var position = Microphone.GetPosition(deviceName);
        var soundData = new float[recordedClip.samples * recordedClip.channels];
        recordedClip.GetData(soundData, 0);

        //Create shortened array for the data that was used for recording
        var newData = new float[position * recordedClip.channels];

        //$$anonymous$$icrophone.End (null);
        //Copy the used samples to a new array
        for (int i = 0; i < newData.Length; i++)
        {
            newData[i] = soundData[i];
        }

        //One does not simply shorten an AudioClip,
        //so we make a new one with the appropriate length
        var newClip = AudioClip.Create(recordedClip.name, position, recordedClip.channels, recordedClip.frequency, false);
        newClip.SetData(newData, 0);        //Give it the data from the old clip

        //Replace the old clip
        AudioClip.Destroy(recordedClip);
        audS.clip = newClip;
        saveCount++;
        string filename = "F:/OneDrive/OneDrive - McGill University/Desktop/" + saveCount.ToString();
        //save and send
        SavWav.Save(filename, CurrentAudioSource.clip);


        Debug.Log("Restart!");
        StartRecording();
    }

}
