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

public class AT_sendAudio : MonoBehaviour
{
        public String Host = "127.0.0.1";// local host
        public Int32 Port = 5000;

        TcpClient mySocket = null;
        NetworkStream theStream = null;
        StreamWriter theWriter = null;

        byte[] AudioBytes ;
        // Start is called before the first frame update = File.ReadAllBytes(@"F:\SRL\SimpleTryOut.wav")
        void Start()
        {
            mySocket = new TcpClient();

        //if (SetupSocket())
        //{
        //    Debug.Log("socket is set up");
        //}
        }

        // Update is called once per frame
        void Update()
        {

            Debug.Log(PlayerPrefs.GetString("ATtheReceivStart"));
            if (PlayerPrefs.GetString("ATtheReceivStart") == "STARTone")
            {
                if (!mySocket.Connected)
                {
                    SetupSocket();
                }
            }
            
            if (PlayerPrefs.GetString("ATtheReceivStart") == "STARTED")
            {
                PlayerPrefs.SetString("ATtheReceivStart", "STARTone");
               

                if (SetupSocket())
                {
                    Debug.Log("socket is set up");
                }
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
        public void SwichScene()
        {
            mySocket.Close();
        }
        private void OnApplicationQuit()
        {
            if (mySocket != null && mySocket.Connected)
                mySocket.Close();
        }

        public void SendWav()
        {
            AudioBytes = File.ReadAllBytes(@"C:\Users\vishav3\Desktop\recordedWav\129.wav");
            mySocket.GetStream().Write(AudioBytes, 0, AudioBytes.Length);
            Debug.Log(AudioBytes.Length);
            Debug.Log(AudioBytes);
            
            Byte[] ByteEnd=Encoding.UTF8.GetBytes("DONE");
            Debug.Log("I am DONE");
            mySocket.GetStream().Write(ByteEnd, 0, ByteEnd.Length);
        }

}
