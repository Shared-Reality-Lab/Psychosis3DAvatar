using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine;
using System.Threading;
using System.Globalization;

public class ReceiveAudioFromPython : MonoBehaviour
{
    Thread mThread;

    // socket settings
    public string connectionIP = "127.0.0.1";
    public int connectionPort;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;

    // audio data
    string msg = "";
    string whole_msg = "";
    float[] float_msg;
    bool running;

    // Create local variables for AudioClip and AudioSource
    AudioClip clip;
    AudioSource audioSource;

    // audio parameters
    int channels = 1;
    int sampleRate = 16000;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("STARTED");
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // if audio data is received, create an audio clip and play the audio 
        if(float_msg != null)
        {
            clip = AudioClip.Create("ClipName", float_msg.Length, channels, sampleRate, false);
            clip.SetData(float_msg, 0);
            audioSource.clip = clip;
            audioSource.Play();
        }

    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(localAdd, connectionPort);
        Debug.Log(localAdd);
        Debug.Log(connectionPort);
        listener.Start();

        client = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            Connection();
        }
    }

    void Connection()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] recevBuffer = new byte[1024];    // set the size of buffer
        int data_len = nwStream.Read(recevBuffer, 0, recevBuffer.Length);
        msg = System.Text.Encoding.UTF8.GetString(recevBuffer, 0, recevBuffer.Length); // byte[] to string

        if (data_len != 0)
        {
            whole_msg += msg;
        }
        else
        {
            if (whole_msg != "")
            {
                whole_msg = whole_msg.Replace("[", "").Replace("]","").Replace("\"","");
                float_msg = Array.ConvertAll(whole_msg.Split(','), float.Parse);
                Debug.Log("THE ORIGINAL MESSAGE : " + whole_msg);

                // reinitialize the variables
                whole_msg = "";
                float_msg = null;
                // close the socket and wait for another client
                client.Close();
                client = listener.AcceptTcpClient();
            }

        }
    }

    void OnApplicationQuit()
    {
        listener.Stop();
        client.Close();
    }
}