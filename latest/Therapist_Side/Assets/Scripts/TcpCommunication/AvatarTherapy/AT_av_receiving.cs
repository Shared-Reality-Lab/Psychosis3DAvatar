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
public class AT_av_receiving : MonoBehaviour
{
    Thread mThread;

    // socket settings
    public string connectionIP = "127.0.0.1";
    public int connectionPort;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    int newClip = 0;
    // audio data
    string msg = "";
    string whole_msg = "";
    float[] float_msg;
    float[] recv_msg = null;
    bool PlayOnce = false;

    // Create local variables for AudioClip and AudioSource
    AudioClip clip;
    AudioSource audioSource;

    // audio parameters
    int channels = 1;
    int sampleRate = 44100;


    byte[] Allcompletedbuffer;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("STARTED");
        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
        //Main();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayOnce)
        {

            PlayOnce = false;
            float_msg = ConvertByteArrayToFloat(Allcompletedbuffer);

            clip = AudioClip.Create("ClipName", float_msg.Length, channels, sampleRate, false);
            clip.SetData(float_msg, 0);
            audioSource.clip = clip;
            audioSource.Play();

            
            Allcompletedbuffer = null;
        }

    }
    static float[] ConvertByteArrayToFloat(byte[] bytes)
    {
        if (bytes.Length % 4 != 0) throw new ArgumentException();

        float[] floats = new float[bytes.Length / 4];
        for (int i = 0; i < floats.Length; i++)
        {
            floats[i] = BitConverter.ToSingle(bytes, i * 4);
        }

        return floats;
    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        Debug.Log(localAdd);
        Debug.Log(connectionPort);
        listener.Start();
        client = listener.AcceptTcpClient();
        Debug.Log("enter GetInfo");
        while (true)
        {
            Connection();
        }
    }
    
    void Connection()
    {
        //Debug.Log("connection");
        List<byte> bigbuffer = new List<byte>();

        byte[] tempbuffer = new byte[1024];
        //can be in another size like 1024 etc.. 
        //depend of the data as you sending from de client
        //i recommend small size for the correct read of the package

        NetworkStream stream = client.GetStream();

        while (stream.Read(tempbuffer, 0, tempbuffer.Length) > 0)
        {
            bigbuffer.AddRange(tempbuffer);
            //Debug.Log("Read");
            if (!stream.DataAvailable)
            {
                break;
            }
        }


        // now you can convert to a native byte array
        byte[] completedbuffer = new byte[bigbuffer.Count];

        bigbuffer.CopyTo(completedbuffer);
        Allcompletedbuffer = completedbuffer;
        PlayOnce = true;
    }
    
    public void SwitchScene()
    {
        listener.Stop();
        client.Close();
    }
    void OnApplicationQuit()
    {
        listener.Stop();
        client.Close();
    }

}