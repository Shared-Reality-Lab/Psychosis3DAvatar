using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading;
using System.Globalization;
using System.IO;

/// <summary>
/// This Client inheritated class acts like Client but using UI elements like buttons and input fields.
/// </summary>
public class CustomClient : Client
{
	[Header("UI References")]
	[SerializeField] private Button m_StartClientButton = null;
	[SerializeField] private Button m_SendToServerButton = null;
	[SerializeField] private InputField m_SendToServerInputField = null;
	[SerializeField] private Button m_SendCloseButton = null;
	[SerializeField] private ScrollRect m_ClientLoggerScrollRect = null;

	private RectTransform m_ClientLoggerRectTransform = null;
	private Text m_ClientLoggerText = null;

	//sending wav
	byte[] AudioBytes ;
	string msg = "";
	string whole_msg = "";
	float[] float_msg;
    bool running;
	Thread mThread;
	// Create local variables for AudioClip and AudioSource
    AudioClip clip;
    AudioSource audioSource;

    // audio parameters
    int channels = 1;
    int sampleRate = 16000;

	//Set UI interactable properties
	private void Awake()
	{
		//Start Client
		m_StartClientButton.onClick.AddListener(base.StartClient);

		//Send to Server
		m_SendToServerButton.interactable = false;
		m_SendToServerButton.onClick.AddListener(SendMessageToServer);

		//SendClose
		m_SendCloseButton.interactable = false;
		m_SendCloseButton.onClick.AddListener(SendCloseToServer);

		//Populate Client delegates
		OnClientStarted = () =>
		{
			//Set UI interactable properties
			m_StartClientButton.interactable = false;
			m_SendToServerButton.interactable = true;
			m_SendCloseButton.interactable = true;
		};

		OnClientClosed = () =>
		{
			//Set UI interactable properties
			m_StartClientButton.interactable = true;
			m_SendToServerButton.interactable = false;
			m_SendCloseButton.interactable = false;
		};

		//UI References
		m_ClientLoggerRectTransform = m_ClientLoggerScrollRect.GetComponent<RectTransform>();
		m_ClientLoggerText = m_ClientLoggerScrollRect.content.gameObject.GetComponent<Text>();
	}
	
	void Update(){
		// if audio data is received, create an audio clip and play the audio 
        if(float_msg != null)
        {
            clip = AudioClip.Create("ClipName", float_msg.Length, channels, sampleRate, false);
            clip.SetData(float_msg, 0);
            audioSource.clip = clip;
            audioSource.Play();
			Debug.Log("float_msg != null");
        }
	}

	private void SendMessageToServer()
	{
		string newMsg = m_SendToServerInputField.text;
		if (string.IsNullOrEmpty(newMsg))
		{
			m_ClientLoggerText.text += $"\n- Enter message";
			return;
		}
		base.SendMessageToServer(newMsg);
	}
	public void PressButtonSend(string msg)
	{
		base.SendMessageToServer(msg);
	}

	private void SendCloseToServer()
	{
		base.SendMessageToServer("Close");
		//Set UI interactable properties
		m_SendCloseButton.interactable = false;
	}
	public void SendWav()
    {
        AudioBytes = File.ReadAllBytes(@"C:/Users/vishav3/Desktop/recordedWav/Send.wav");
        //Debug.Log("I am here");
        base.m_Client.GetStream().Write(AudioBytes, 0, AudioBytes.Length);
        Byte[] ByteEnd=Encoding.UTF8.GetBytes("DONE");
        //Debug.Log("I am DONE");
        base.m_Client.GetStream().Write(ByteEnd, 0, ByteEnd.Length);
        audioSource = GetComponent<AudioSource>();
        //Debug.Log("STARTED");
         
    }

	public void StartGetInfo(){
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
			Debug.Log("GetInfo");
			running = true;
			while (running)
			{
				Connection();
			}
		}
	void Connection()
		{
			Debug.Log("Connection");
			NetworkStream nwStream = base.m_NetStream;
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


	//Custom Client Log
	#region ClientLog
	protected override void ClientLog(string msg)
	{
		base.ClientLog(msg);
		m_ClientLoggerText.text += $"\n- {msg}";

		//Ensure ScrollBar shows last message
		LayoutRebuilder.ForceRebuildLayoutImmediate(m_ClientLoggerRectTransform);
		m_ClientLoggerScrollRect.verticalNormalizedPosition = 0f;
	}
	protected override void ClientLog(string msg, Color color)
	{
		base.ClientLog(msg, color);
		m_ClientLoggerText.text += $"\n<color=#{ColorUtility.ToHtmlStringRGBA(color)}>- {msg}</color>";

		//Ensure ScrollBar shows last message
		LayoutRebuilder.ForceRebuildLayoutImmediate(m_ClientLoggerRectTransform);
		m_ClientLoggerScrollRect.verticalNormalizedPosition = 0f;
	}
	#endregion
}