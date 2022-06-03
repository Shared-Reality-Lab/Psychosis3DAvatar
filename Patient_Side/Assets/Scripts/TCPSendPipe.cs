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
using UnityEngine.UI;

public class TCPSendPipe : MonoBehaviour
{
    public string Host;// local host
    public int Port;
    public Text ShowRecord;

    TcpClient mySocket = null;
    NetworkStream theStream = null;
    StreamWriter theWriter = null;
    string P_server;

    byte[] AudioBytes ;
    //////////////Pause Detection//////////////////////
    public int pauselength;
    AudioSource CurrentAudioSource;
    AudioClip audio_;
    bool recording=false;
    int sample_size = 128;
    float timer = 0.0f;
    float seconds= 0.0f;
    int saveCount = 0;

    //////////////////Reciever//////////////////////////////
    Thread mThread;
    // audio data
    string msg = "";
    string whole_msg = "";
    float[] float_msg;
    bool running;
    bool NotStart= true;
    IPAddress localAdd;
    // Create local variables for AudioClip and AudioSource
    AudioClip clip;
    public AudioSource audioSource;

    // audio parameters
    public string Sendfilename = @"C:\Users\vishav3\Desktop\Send\send.wav";
    public string filename = @"C:\Users\vishav3\Desktop\Send\send";
    int channels = 1;
    int sampleRate = 16000;
    
	bool waitPlay=false;
    bool startPlay = false;
    // Start is called before the first frame update = File.ReadAllBytes(@"F:\SRL\SimpleTryOut.wav")
    void Start()
    {
        NotStart = true;
        CurrentAudioSource = GetComponent<AudioSource>();
        audio_ = CurrentAudioSource.clip;
        ////////////
        mySocket = new TcpClient();
        P_server=PlayerPrefs.GetString("Server");
		waitPlay=false;
        startPlay = false;
        if (P_server == "Unicorn")//Add connection here
        {
            Debug.Log("IP is Unicorn");
            Host = "132.206.74.92";
            Port = 8000;
        }

        if (P_server == "Pegasus")//Add connection here
        {
            Debug.Log("IP is Pegasus");
            Host = "132.206.74.89";
            Port = 8000;
        }
        if (SetupSocket())
        {
            Debug.Log("socket is set up");
        }
    }
    void FixedUpdate()
    {
        

        if (recording)
        {
            ShowRecord.enabled=true;
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
                if (peak > 4E-05)
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
                        StartCoroutine(EndRecording(CurrentAudioSource, ""));
						seconds= 0.0f;
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
    
    void StartRecording()
    {
        Debug.Log("StartRecording");
        timer = 0.0f;
        CurrentAudioSource.clip = Microphone.Start(null, false, 300, 44100);
        recording = true;
        
    }

    IEnumerator EndRecording(AudioSource audS, string deviceName)
    {
        Debug.Log("EndRecording");
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
        //string filename = "C:/Users/vishav3/Desktop/recordedWav/send";
        //save and send
        SavWav.Save(filename, CurrentAudioSource.clip);
        SendWav();
		yield return null;
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
        if(float_msg != null)
        {
            clip = AudioClip.Create("ClipName", float_msg.Length, channels, sampleRate, false);
            clip.SetData(float_msg, 0);
            audioSource.clip = clip;
            audioSource.Play();
			whole_msg = "";
			float_msg= null;
        }
        if (audioSource.isPlaying)
        {
            Debug.Log("isPlaying");
            startPlay = true;
        }
        if (waitPlay&& startPlay &&(!audioSource.isPlaying))
		{
			Debug.Log("waitPlay========================================================");
			waitPlay=false;
            startPlay = false;
            //SetUpSocket();
            StartRecording();
		}
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
        //string Sendfilename = @"C:/Users/vishav3/Desktop/recordedWav/send.wav";
        AudioBytes = File.ReadAllBytes(@Sendfilename);
        //Debug.Log("I am here");
        mySocket.GetStream().Write(AudioBytes, 0, AudioBytes.Length);
        Byte[] ByteEnd=Encoding.UTF8.GetBytes("DONE");
        //Debug.Log("I am DONE");
        mySocket.GetStream().Write(ByteEnd, 0, ByteEnd.Length);
        //audioSource = GetComponent<AudioSource>();
        Debug.Log("ThreadStart STARTED");
        StartNewThread();   
    }

    void StartNewThread()
    {
        if (NotStart)
        {
            ThreadStart ts = new ThreadStart(GetInfo);
            mThread = new Thread(ts);
            mThread.Start();
            Debug.Log("StartNewThread");
            NotStart = false;
        }

    }
    void GetInfo()
    {
        running = true;
        while (running)
        {
            Connection();
        }
    }
    //void SetUpSocket()
    //{
    //    mySocket = new TcpClient();
    //    SetupSocket();
    //}
    void Connection()
    {
        Debug.Log("ENTER Connection");
        NetworkStream nwStream = mySocket.GetStream();
        byte[] recevBuffer = new byte[1024];    // set the size of buffer
        float data_len = nwStream.Read(recevBuffer, 0, recevBuffer.Length);
        float[] tmp_float_msg;
        msg = System.Text.Encoding.UTF8.GetString(recevBuffer, 0, recevBuffer.Length); // byte[] to string
        //Debug.Log("I am message");
        if (data_len != 0)
        {
            Debug.Log("data_len != 0");
            whole_msg += msg;
            //Debug.Log(whole_msg);
        }
        else
        {
            Debug.Log("BEFORE IF");
            if (whole_msg != "")
            {
                // close the socket and wait for another client
                mySocket.Close();
                // mySocket = listener.AcceptTcpClient();
                //startThread = 0;
                mySocket = new TcpClient();
                SetupSocket();

                Debug.Log("AFTER IF");

                whole_msg = whole_msg.Replace("[", "").Replace("]", "").Replace("\"", "");
                tmp_float_msg = Array.ConvertAll(whole_msg.Split(','), float.Parse);//float_msg = Array.ConvertAll(whole_msg.Split(','), float.Parse);
                Debug.Log("THE ORIGINAL MESSAGE : " + whole_msg);
                Debug.Log("I am not empty");
                // reinitialize the variables
                //whole_msg = "";
                msg = "";
                float_msg = new float[tmp_float_msg.Length];
                float_msg = tmp_float_msg;
                waitPlay = true;

            }

        }
    }

}