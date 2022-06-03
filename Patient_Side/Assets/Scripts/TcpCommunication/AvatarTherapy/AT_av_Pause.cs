using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine;
using System.Threading;
using System.Globalization;
using System.IO;

public class AT_av_Pause : MonoBehaviour
{
    public string Host;// local host
    public int Port;
    public AT_av_serverR other;
    TcpClient mySocket = null;
    NetworkStream theStream = null;
    StreamWriter theWriter = null;
    string P_server;

    byte[] AudioBytes;
    //////////////Pause Detection//////////////////////
    public int pauselength;
    AudioSource CurrentAudioSource;
    AudioClip audio_;
    bool recording = false;
    int sample_size = 128;
    float timer = 0.0f;
    float seconds = 0.0f;
    int saveCount = 0;

    //////////////////Reciever//////////////////////////////
    Thread mThread;
    // audio data
    string msg = "";
    string whole_msg = "";
    float[] float_msg;
    bool running;
    IPAddress localAdd;
    // Create local variables for AudioClip and AudioSource
    AudioClip clip;
    public AudioSource audioSource;

    // audio parameters
    int channels = 1;
    int sampleRate = 16000;
    //public GameObject ReceiveReference;
    //private AT_av_serverR bool_waitPlay;
    //bool waitPlay;
    //bool startPlay = false;

    // Start is called before the first frame update = File.ReadAllBytes(@"F:\SRL\SimpleTryOut.wav")
    void Start()
    {

        CurrentAudioSource = GetComponent<AudioSource>();
        audio_ = CurrentAudioSource.clip;
        ////////////
        mySocket = new TcpClient();
        P_server = PlayerPrefs.GetString("server");

        // bool_waitPlay = ReceiveReference.GetComponent<AT_av_serverR>();
        // waitPlay=bool_waitPlay.waitPlay;
        // startPlay = false;132.206.74.173127.0.0.1

        Host = "127.0.0.1";
        Port = 6000;
    }
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
                Debug.Log("Peak");
                Debug.Log(peak);
                if (peak > 4E-04)
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
                    if (seconds > pauselength)
                    {
                        Debug.Log("Pause! EndRecording");
                        StartCoroutine(EndRecording(CurrentAudioSource, ""));
                        seconds = 0.0f;
                        timer = 0.0f;
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
        StartCoroutine(EndRecording(CurrentAudioSource, ""));

    }

    public void PressPlay()
    {

        CurrentAudioSource.Play();
    }

    public void StartRecording()
    {
        Debug.Log("StartRecording");
        timer = 0.0f;
        CurrentAudioSource.clip = Microphone.Start(null, false, 300, 44100);
        recording = true;

    }

    IEnumerator EndRecording(AudioSource audS, string deviceName)
    {
        
        Debug.Log("EndRecording");
        other.StartNewThread();
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
        string filename = "C:/Users/vishav3/Desktop/recordedWav/128";
        //save and send
        SavWav.Save(filename, CurrentAudioSource.clip);
        SendWav();
        yield return null;
        /*while(waitPlay)
		{
			waitPlay=false;
			Debug.Log("Restart!");
			StartRecording();
			Debug.Log("yield return null!");
			yield return null;

		}*/

        /*
        while((audioSource.isPlaying)||(float_msg == null))
        {
            Debug.Log("yield return null!");
			yield return null;
        }
        Debug.Log("Restart!");
        StartRecording();
		*/
    }
    /////////////////////////////////////////////////////////////////////////////////////
    // Update is called once per frame
    void Update()
    {
        if (!mySocket.Connected)
        {
            SetupSocket();
        }
        // if audio data is received, create an audio clip and play the audio 
        /*if (float_msg != null)
        {
            clip = AudioClip.Create("ClipName", float_msg.Length, channels, sampleRate, false);
            clip.SetData(float_msg, 0);
            audioSource.clip = clip;
            audioSource.Play();
            whole_msg = "";
            float_msg = null;
        }*/
        // if (audioSource.isPlaying)
        // {
        //     Debug.Log("isPlaying");
        //     startPlay = true;
        // }
        // if (waitPlay&& startPlay &&(!audioSource.isPlaying))
		// {
		// 	Debug.Log("waitPlay========================================================");
		// 	waitPlay=false;
        //     startPlay = false;
        //     //SetUpSocket();
        //     StartRecording();
		// }
        // if(waitPlay){
        //     Debug.Log("waitPlayTRUE========================================================");
        // }
        // else{
        //     Debug.Log("waitPlayFALSE========================================================");
        // }
        // if(startPlay){
        //     Debug.Log("startPlayTRUE========================================================");
        // }
        // else{
        //      Debug.Log("startPlayFALSE========================================================");
        // }
        // if(audioSource.isPlaying){
        //     Debug.Log("audioSource.isPlayingTRUE========================================================");
        // }
        // else{
        //     Debug.Log("audioSource.isPlayingFALSE========================================================");
        // }
    }

    public bool SetupSocket()
    {
        try
        {
            mySocket.Connect(Host, Port);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
            Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes("yah!! it works");
            //mySocket.GetStream().Write(sendBytes, 0, sendBytes.Length);
            Debug.Log("socket is sent");
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e);
            return false;
        }
    }

    private void OnApplicationQuit()
    {
        if (mySocket != null && mySocket.Connected)
            mySocket.Close();

    }

    void SendWav()
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!ENTERING SEND WAV!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        string Sendfilename = @"C:/Users/vishav3/Desktop/recordedWav/128.wav";
        AudioBytes = File.ReadAllBytes(@Sendfilename);
        //Debug.Log("I am here");
        mySocket.GetStream().Write(AudioBytes, 0, AudioBytes.Length);
        Byte[] ByteEnd = Encoding.UTF8.GetBytes("DONE");
        //Debug.Log("I am DONE");
        mySocket.GetStream().Write(ByteEnd, 0, ByteEnd.Length);
        //audioSource = GetComponent<AudioSource>();
        Debug.Log("ThreadStart STARTED");
        //ThreadStart ts = new ThreadStart(GetInfo);
        //mThread = new Thread(ts);
        //mThread.Start();
    }
/*
    void GetInfo()
    {
        // localAdd = IPAddress.Parse(Host);
        // listener = new TcpListener(localAdd, Port);
        // Debug.Log(localAdd);
        // Debug.Log("Port");
        // Debug.Log(Port);
        // listener.Start();

        // mySocket = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            Connection();
        }
    }
*/
/*    void Connection()
    {
        NetworkStream nwStream = mySocket.GetStream();
        byte[] recevBuffer = new byte[1024];    // set the size of buffer
        float data_len = nwStream.Read(recevBuffer, 0, recevBuffer.Length);
        float[] tmp_float_msg;
        msg = System.Text.Encoding.UTF8.GetString(recevBuffer, 0, recevBuffer.Length); // byte[] to string
        //Debug.Log("I am message");
        if (data_len != 0)
        {
            whole_msg += msg;
            Debug.Log(whole_msg);
        }
        else
        {
            if (whole_msg != "")
            {
                
				whole_msg = whole_msg.Replace("[", "").Replace("]","").Replace("\"","");
                float_msg = Array.ConvertAll(whole_msg.Split(','), float.Parse);
                Debug.Log("THE ORIGINAL MESSAGE : " + whole_msg);
                Debug.Log("I am not empty");
                // reinitialize the variables
                whole_msg = "";
                float_msg = null;
                 
                // close the socket and wait for another client
                // mySocket.Close();
                // mySocket = listener.AcceptTcpClient();
				


                // close the socket and wait for another client
                mySocket.Close();
                // mySocket = listener.AcceptTcpClient();
                mySocket = new TcpClient();
                SetupSocket();

                whole_msg = whole_msg.Replace("[", "").Replace("]", "").Replace("\"", "");
                tmp_float_msg = Array.ConvertAll(whole_msg.Split(','), float.Parse);//float_msg = Array.ConvertAll(whole_msg.Split(','), float.Parse);
                Debug.Log("THE ORIGINAL MESSAGE : " + whole_msg);
                Debug.Log("I am not empty");
                // reinitialize the variables
                //whole_msg = "";
                msg = "";
                float_msg = new float[tmp_float_msg.Length];
                float_msg = tmp_float_msg;
                Debug.Log("waitPlay=true;");
                waitPlay = true;

            }

        }
    }
    */
}
