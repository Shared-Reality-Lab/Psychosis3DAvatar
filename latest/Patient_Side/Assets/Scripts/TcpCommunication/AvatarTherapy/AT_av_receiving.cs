using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using UnityEngine;
using System.Threading;
public class AT_av_receiving : MonoBehaviour
{
    //right now we are using the original method, not including the pause detection one.
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
    float[] recv_msg= null;
    bool PlayOnce=false;
    
    // Create local variables for AudioClip and AudioSource
    AudioClip clip;
    AudioSource audioSource;

    // audio parameters
    int channels = 1;
    int sampleRate = 44100;

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
        if (float_msg != null&& PlayOnce)
        {
            clip = AudioClip.Create("ClipName", float_msg.Length, channels, sampleRate, false);
            //clip.GetData(float_msg, 0);
            clip.SetData(float_msg, 0);
            audioSource.clip = clip;
            audioSource.Play();
            PlayOnce = false;
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

        Debug.Log("enter GetInfo");
        while (true)
        {
            Connection();
        }
    }
    public float[] ConvertByteToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 4];
        Buffer.BlockCopy(array, 0, floatArr, 0, array.Length);
        //for (int i = 0; i < floatArr.Length; i++)
        //{
        //    if (BitConverter.IsLittleEndian)
        //        Array.Reverse(array, i * 4, 4);
        //    floatArr[i] = BitConverter.ToSingle(array, i * 4) / 0x80000000;
        //    //Debug.Log(floatArr[i]);
        //}
        //Debug.Log(floatArr);
        //Debug.Log(":I am float Array");
        return floatArr;

    }
    void Connection()
    {
        Debug.Log("connection");
        List<byte> bigbuffer = new List<byte>();

        byte[] tempbuffer = new byte[1024];
        //can be in another size like 1024 etc.. 
        //depend of the data as you sending from de client
        //i recommend small size for the correct read of the package

        NetworkStream stream = client.GetStream();

        while (stream.Read(tempbuffer, 0, tempbuffer.Length) > 0)
        {
            bigbuffer.AddRange(tempbuffer);
            Debug.Log("Read");
            if (!stream.DataAvailable)
            {
                break;
            }
        }


        // now you can convert to a native byte array
        byte[] completedbuffer = new byte[bigbuffer.Count];

        bigbuffer.CopyTo(completedbuffer);

        //Do something with the data
        //string decodedmsg = Encoding.ASCII.GetString(completedbuffer);
        Debug.Log("length" + completedbuffer.Length);
        float_msg = ConvertByteToFloat(completedbuffer);
        PlayOnce = true;

    }


    void OnApplicationQuit()
    {
        listener.Stop();
        client.Close();
    }
}
