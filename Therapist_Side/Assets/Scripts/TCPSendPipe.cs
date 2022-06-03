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


public class TCPSendPipe : MonoBehaviour
{
    public GameObject recording;
    
    public string Host;// local host
    public int Port;

    TcpClient mySocket = null;
    NetworkStream theStream = null;
    StreamWriter theWriter = null;
    string P_server;

    byte[] AudioBytes ;
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
    AudioSource audioSource;

    // audio parameters
    int channels = 1;
    int sampleRate = 16000;

    // Start is called before the first frame update = File.ReadAllBytes(@"F:\SRL\SimpleTryOut.wav")
    void Start()
    {
        mySocket = new TcpClient();
        P_server=PlayerPrefs.GetString("server");

        if (P_server == "Unicorn")//Add connection here
        {
            Debug.Log("CCCCC");
            Host = "132.206.74.92";
            Port = 8000;
        }

        if (P_server == "Bach")//Add connection here
        {
            Debug.Log("BBBBBB");
            Host = "132.206.74.208";
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
        }
    }
//selection Avatar mode or therapy mode
    public void AvatarMode(){
        Debug.Log("AvatarMode");
        if (P_server == "Unicorn")//Add connection here
        {
            Debug.Log("CCCCC");
            Host = "132.206.74.92";
            Port = 8000;
        }

        if (P_server == "Bach")//Add connection here
        {
            Debug.Log("BBBBBB");
            Host = "132.206.74.208";
            Port = 8000;
        }

        recording.SetActive(true);
    }

    public void TherapyMode(){
        Debug.Log("TherapyMode");
        Host = "127.0.0.1";
        Port = 5000;

        recording.SetActive(true);
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
    
    public void SendWav()
    {
        AudioBytes = File.ReadAllBytes(@"C:/Users/vishav3/Desktop/recordedWav/Send.wav");
        //Debug.Log("I am here");
        mySocket.GetStream().Write(AudioBytes, 0, AudioBytes.Length);
        Byte[] ByteEnd=Encoding.UTF8.GetBytes("DONE");
        //Debug.Log("I am DONE");
        mySocket.GetStream().Write(ByteEnd, 0, ByteEnd.Length);
        audioSource = GetComponent<AudioSource>();
        //Debug.Log("STARTED");
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();    
    }
     void GetInfo()
    {
        // // localAdd = IPAddress.Parse(Host);
        // listener = new TcpListener(mySocket);
        // // Debug.Log(localAdd);
        // // Debug.Log(Port);
        // listener.Start();

        // client = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            Connection();
        }
    }

    void Connection()
    {
        NetworkStream nwStream = mySocket.GetStream();
        byte[] recevBuffer = new byte[1024];    // set the size of buffer
        int data_len = nwStream.Read(recevBuffer, 0, recevBuffer.Length);
        msg = System.Text.Encoding.UTF8.GetString(recevBuffer, 0, recevBuffer.Length); // byte[] to string
        //Debug.Log("I am message");
        if (data_len != 0)
        {
            whole_msg += msg;
            //Debug.Log(whole_msg);
        }
        else
        {
            if (whole_msg != "")
            {
                whole_msg = whole_msg.Replace("[", "").Replace("]","").Replace("\"","");
                float_msg = Array.ConvertAll(whole_msg.Split(','), float.Parse);
                Debug.Log("THE ORIGINAL MESSAGE : " + whole_msg);
               // Debug.Log("I am not empty");
                // reinitialize the variables
                whole_msg = "";
                float_msg = null;
                // close the socket and wait for another client
                // mySocket.Close();
                // mySocket = listener.AcceptTcpClient();
            }

        }
    }

}
