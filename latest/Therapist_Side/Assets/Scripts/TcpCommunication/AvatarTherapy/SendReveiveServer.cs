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
public class SendReveiveServer : MonoBehaviour
{
    public Dropdown SelectVoice;

    string Host;// local host
    int Port;

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
        P_server=PlayerPrefs.GetString("Server");

        if (P_server == "Unicorn")//Add connection here
        {
            Debug.Log("IP is Unicorn");
            Host = "132.206.74.92";
            Port = 8000;
        }

        if (P_server == "Pegasus")//Add connection here
        {
            Debug.Log("IP is Bach");
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
            Debug.Log("!mySocket.Connected");
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
    
    public void SendWav()
    {
        AudioBytes = File.ReadAllBytes(@"D:\SRL\audio\129.wav");//
        Debug.Log("I am here");
        Byte[] sendSelection;
        if (PlayerPrefs.GetString("Voice")  == "Male")
            sendSelection = Encoding.UTF8.GetBytes("AAAA");
        else
            sendSelection = Encoding.UTF8.GetBytes("BBBB");
        mySocket.GetStream().Write(sendSelection, 0, sendSelection.Length);

        mySocket.GetStream().Write(AudioBytes, 0, AudioBytes.Length);

        Byte[] ByteEnd=Encoding.UTF8.GetBytes("DONE");
        Debug.Log("I am DONE");
        mySocket.GetStream().Write(ByteEnd, 0, ByteEnd.Length);

        mySocket.Close();
        mySocket = new TcpClient();
        SetupSocket();


        //audioSource = GetComponent<AudioSource>();
        //Debug.Log("STARTED");
        //ThreadStart ts = new ThreadStart(GetInfo);
        //mThread = new Thread(ts);
        //mThread.Start();    
    }
    
}
