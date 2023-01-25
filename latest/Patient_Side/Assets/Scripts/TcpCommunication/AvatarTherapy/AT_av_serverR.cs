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
using System.Linq;

public class AT_av_serverR : MonoBehaviour
{
    string Host;// local host
    int Port;

    TcpClient mySocket = null;
    NetworkStream theStream = null;
    StreamWriter theWriter = null;
    string P_server;

    byte[] AudioBytes;
    //////////////Pause Detection//////////////////////
    //public int pauselength;
    AudioSource CurrentAudioSource;

    AudioClip audio_;
    bool recording = false;
    int sample_size = 128;
    float timer = 0.0f;
    float seconds;
    int saveCount = 0;
    bool CanPlay = false;

    // Create local variables for AudioClip and AudioSource
    AudioClip clip;
    public AudioSource audioSource;
    public AudioSource audioSource2;

    // audio parameters
    int channels = 1;
    int sampleRate = 16000;


    //////////////////Reciever//////////////////////////////
    Thread mThread;
    // audio data
    string msg = "";
    string whole_msg = "";
    float[] float_msg;
    bool running;
    IPAddress localAdd;
    /// <summary>
    /// ///
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    /// </summary>
    // Start is called before the first frame update = File.ReadAllBytes(@"F:\SRL\SimpleTryOut.wav")
    void Start()
    {
        ConnectToTcpServer();
    }

    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
            Debug.Log("connect server ");
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }

    void Update()
    {
        if (float_msg != null && CanPlay)
        {
            Debug.Log("float_msgg.Length " + float_msg.Length);
            Debug.Log("float_msg !=null " + float_msg);

            foreach (float value in float_msg)
            {
                print(value);
            }

            clip = AudioClip.Create("ClipName", float_msg.Length, channels, sampleRate, false);
            clip.SetData(float_msg, 0);
            audioSource.clip = clip;
            audioSource.Play();
            audioSource2.clip = clip;
            audioSource2.Play();
            float_msg = null;
            whole_msg = "";
            CanPlay = false;
        }
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    public void ListenForData()
    {
        float[] float_msg_temp;
        float[] float_ms_z;
        float[] float_try = { 3.0f, 3.5F, 4.0F, 4.5F, 5.0F };

        socketConnection = new TcpClient("132.206.74.92", 8000);
        NetworkStream myNetworkStream = socketConnection.GetStream();

        while (true)
        {
            
            
            
            if (myNetworkStream.CanRead)
            {

                // Incoming message may be larger than the buffer size.
                int count = 0;

                do
                {
                    //numberOfBytesRead = myNetworkStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                    byte[] myReadBuffer = new byte[1024];
                    float data_len = myNetworkStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                    StringBuilder myCompleteMessage = new StringBuilder();
                    int numberOfBytesRead = 0;

                    float_msg_temp = ConvertByteArrayToFloat(myReadBuffer);
                    if (float_msg != null)
                    {
                        Debug.Log("if (float_msg != null)  ");
                        float_ms_z = new float[float_msg.Length + float_msg_temp.Length];
                        float_msg.CopyTo(float_ms_z, 0);
                        float_msg_temp.CopyTo(float_ms_z, float_msg.Length);
                        float_msg = float_ms_z;
                        Debug.Log("Updating float_msgg.Length " + float_msg.Length);
                        count++;
                        Debug.Log("count---------- " + count);
                    }

                    else
                    {
                        Debug.Log("else if (float_msg != null)  ");
                        float_ms_z = float_msg_temp;
                        float_msg = float_ms_z;
                        Debug.Log("Initial float_msgg.Length " + float_msg.Length);
                    }
                    if (!myNetworkStream.DataAvailable)
                        System.Threading.Thread.Sleep(1);

                } while (myNetworkStream.DataAvailable);
                socketConnection.Close();
                Debug.Log("END！server message received as: " + float_msg);
                CanPlay = true;

                socketConnection = new TcpClient("132.206.74.92", 8000);
                myNetworkStream = socketConnection.GetStream();
                Debug.Log("reconnnect ");

                // Print out the received message to the console.
                //Console.WriteLine("You received the following message : " + myCompleteMessage);
            }
            else
            {
                //Console.WriteLine("Sorry.  You cannot read from this NetworkStream.");
                Debug.Log("Sorry.  You cannot read from this NetworkStream.");
            }
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

}