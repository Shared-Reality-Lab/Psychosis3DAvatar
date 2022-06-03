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


public class AT_av_serverR : MonoBehaviour
{
    string Host;// local host
    int Port;

    TcpClient mySocket = null;
    NetworkStream theStream = null;
    StreamWriter theWriter = null;
    string P_server;

    byte[] AudioBytes ;
    //////////////Pause Detection//////////////////////
    //public int pauselength;
    AudioSource CurrentAudioSource;
    AudioClip audio_;
    bool recording=false;
    int sample_size = 128;
    float timer = 0.0f;
    float seconds;
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

    //boolean judgementation
    bool isSocketStart = false;
    int startThread=0;

	bool NotStart=true;
    // restart pause
    public bool waitPlay=false;
    bool startPlay = false;
    public GameObject ReceiveReference;
    private AT_av_Pause startControl;
    // Start is called before the first frame update = File.ReadAllBytes(@"F:\SRL\SimpleTryOut.wav")
    void Start()
    {
        NotStart=true;
		startThread=0;
        startControl = ReceiveReference.GetComponent<AT_av_Pause>();
        //CurrentAudioSource = GetComponent<AudioSource>();
        //audio_ = CurrentAudioSource.clip;
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
			float_msg = null;
			whole_msg = "";
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
            startControl.StartRecording();
		}


    }

	public void StartNewThread()
	{
		if(NotStart)
		{
			ThreadStart ts = new ThreadStart(GetInfo);
			mThread = new Thread(ts);
			mThread.Start();
			Debug.Log("StartNewThread");
			NotStart=false;
		}
		
	}

    public bool SetupSocket()
    {
        try
        {
            mySocket.Connect(Host, Port);
            //theStream = mySocket.GetStream();
            //theWriter = new StreamWriter(theStream);
            //Byte[] sendBytes = System.Text.Encoding.UTF8.GetBytes("yah!! it works");
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
   

    void GetInfo()
    {


        running = true;
        while (running)
        {
            Connection();
        }
    }

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
				startThread=0;
                mySocket = new TcpClient();
                SetupSocket();

				Debug.Log("AFTER IF");

                whole_msg = whole_msg.Replace("[", "").Replace("]","").Replace("\"","");
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