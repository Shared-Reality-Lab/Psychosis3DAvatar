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

public class AT_the_receiving1 : MonoBehaviour
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
    float[] float_msg= null;
    float[] recv_msg= null;
    bool PlayOnce=false;
    bool isThreadStart=false;
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
		isThreadStart=false;
        
        PlayOnce = false;
		
        //Main();
    }
  
    // Update is called once per frame
    void Update()
    {
        Debug.Log(PlayerPrefs.GetString("ATtheReceiveStart"));
		if (PlayerPrefs.GetString("ATtheReceiveStart") == "STARTED")
		{
			PlayerPrefs.SetString("ATtheReceiveStart", "STARTOne");
			ThreadStart ts = new ThreadStart(GetInfo);
			mThread = new Thread(ts);
			mThread.Start();
		}
		if ( PlayOnce)
        {
            // clip = AudioClip.Create("ClipName", float_msg.Length, channels, sampleRate, false);
            // // clip.GetData(float_msg, 0);
            // clip.SetData(float_msg, 0);
            // audioSource.clip = clip;
            // audioSource.Play();
            PlayOnce = false;
            audioSource.clip = FromPcmBytes(Allcompletedbuffer,"pcm");
            audioSource.Play();
			Allcompletedbuffer=null;
        }
        if (isThreadStart)
        {
            if (PlayerPrefs.GetString("Serverreceive") == "ATAvatar")
            {
                Debug.Log("re_Switch ATAvatar");
                listener.Stop();
                client.Close();
            }
            if (PlayerPrefs.GetString("Serverreceive") == "EndSession")
            {
                Debug.Log("re_Switch EndSession");
                listener.Stop();
                client.Close();
            }
        }
		

    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);//localAdd
        Debug.Log(localAdd);
        Debug.Log(connectionPort);
        listener.Start();
        client = listener.AcceptTcpClient();

        Debug.Log("enter GetInfo");
        isThreadStart = true;
        while (true)
        {
            Connection();
        }
    }
    public float[] ConvertByteToFloat(byte[] array)
    {
        float[] floatArr = new float[array.Length / 4];
        Buffer.BlockCopy(array, 0, floatArr, 0, array.Length);
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
		Allcompletedbuffer=completedbuffer;
        //Do something with the data
        //string decodedmsg = Encoding.ASCII.GetString(completedbuffer);
        Debug.Log("length" + completedbuffer.Length);
        //float_msg = ConvertByteToFloat(completedbuffer);
        PlayOnce = true;
        //new
        //float_msg = null;
    }


	    private readonly struct PcmHeader
{
    #region Public types & data

    public int    BitDepth         { get; }
    public int    AudioSampleSize  { get; }
    public int    AudioSampleCount { get; }
    public ushort Channels         { get; }
    public int    SampleRate       { get; }
    public int    AudioStartIndex  { get; }
    public int    ByteRate         { get; }
    public ushort BlockAlign       { get; }

    #endregion

    #region Constructors & Finalizer

    private PcmHeader(int bitDepth,
        int               audioSize,
        int               audioStartIndex,
        ushort            channels,
        int               sampleRate,
        int               byteRate,
        ushort            blockAlign)
    {
        BitDepth       = bitDepth;
        _negativeDepth = Mathf.Pow(2f, BitDepth - 1f);
        _positiveDepth = _negativeDepth - 1f;

        AudioSampleSize  = bitDepth / 8;
        AudioSampleCount = Mathf.FloorToInt(audioSize / (float)AudioSampleSize);
        AudioStartIndex  = audioStartIndex;

        Channels   = channels;
        SampleRate = sampleRate;
        ByteRate   = byteRate;
        BlockAlign = blockAlign;
    }

    #endregion

    #region Public Methods

    public static PcmHeader FromBytes(byte[] pcmBytes)
    {
        using var memoryStream = new MemoryStream(pcmBytes);
        return FromStream(memoryStream);
    }

    public static PcmHeader FromStream(Stream pcmStream)
    {
        pcmStream.Position = SizeIndex;
        using BinaryReader reader = new BinaryReader(pcmStream);

        int    headerSize      = reader.ReadInt32();  // 16
        ushort audioFormatCode = reader.ReadUInt16(); // 20

        string audioFormat = GetAudioFormatFromCode(audioFormatCode);
        if (audioFormatCode != 1 && audioFormatCode == 65534)
        {
            // Only uncompressed PCM wav files are supported.
            throw new ArgumentOutOfRangeException(nameof(pcmStream),
                                                  $"Detected format code '{audioFormatCode}' {audioFormat}, but only PCM and WaveFormatExtensible uncompressed formats are currently supported.");
        }

        ushort channelCount = reader.ReadUInt16(); // 22
        int    sampleRate   = reader.ReadInt32();  // 24
        int    byteRate     = reader.ReadInt32();  // 28
        ushort blockAlign   = reader.ReadUInt16(); // 32
        ushort bitDepth     = reader.ReadUInt16(); //34

        pcmStream.Position = SizeIndex + headerSize + 2 * sizeof(int); // Header end index
        int audioSize = reader.ReadInt32();                            // Audio size index

        return new PcmHeader(bitDepth, audioSize, (int)pcmStream.Position, channelCount, sampleRate, byteRate, blockAlign); // audio start index
    }

    public float NormalizeSample(float rawSample)
    {
        float sampleDepth = rawSample < 0 ? _negativeDepth : _positiveDepth;
        return rawSample / sampleDepth;
    }

    #endregion

    #region Private Methods

        private static string GetAudioFormatFromCode(ushort code)
        {
            switch (code)
            {
                case 1:     return "PCM";
                case 2:     return "ADPCM";
                case 3:     return "IEEE";
                case 7:     return "?-law";
                case 65534: return "WaveFormatExtensible";
                default:    throw new ArgumentOutOfRangeException(nameof(code), code, "Unknown wav code format.");
            }
        }

        #endregion

        #region Private types & Data

        private const int SizeIndex = 16;

        private readonly float _positiveDepth;
        private readonly float _negativeDepth;

        #endregion
    }
    private readonly struct PcmData
    {
        #region Public types & data

        public float[] Value      { get; }
        public int     Length     { get; }
        public int     Channels   { get; }
        public int     SampleRate { get; }

        #endregion

        #region Constructors & Finalizer

        private PcmData(float[] value, int channels, int sampleRate)
        {
            Value      = value;
            Length     = value.Length;
            Channels   = channels;
            SampleRate = sampleRate;
        }

        #endregion

        #region Public Methods

        public static PcmData FromBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            PcmHeader pcmHeader = PcmHeader.FromBytes(bytes);
            if (pcmHeader.BitDepth != 16 && pcmHeader.BitDepth != 32 && pcmHeader.BitDepth != 8)
            {
                throw new ArgumentOutOfRangeException(nameof(pcmHeader.BitDepth), pcmHeader.BitDepth, "Supported values are: 8, 16, 32");
            }

            float[] samples = new float[pcmHeader.AudioSampleCount];
            for (int i = 0; i < samples.Length; ++i)
            {
                int   byteIndex = pcmHeader.AudioStartIndex + i * pcmHeader.AudioSampleSize;
                float rawSample;
                switch (pcmHeader.BitDepth)
                {
                    case 8:
                        rawSample = bytes[byteIndex];
                        break;

                    case 16:
                        rawSample = BitConverter.ToInt16(bytes, byteIndex);
                        break;

                    case 32:
                        rawSample = BitConverter.ToInt32(bytes, byteIndex);
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(pcmHeader.BitDepth), pcmHeader.BitDepth, "Supported values are: 8, 16, 32");
                }

                samples[i] = pcmHeader.NormalizeSample(rawSample); // normalize sample between [-1f, 1f]
            }

            return new PcmData(samples, pcmHeader.Channels, pcmHeader.SampleRate);
        }

        #endregion
    }
    public static AudioClip FromPcmBytes(byte[] bytes, string clipName = "pcm")
    {
    // clipName.ThrowIfNullOrWhitespace(nameof(clipName));
        var pcmData   = PcmData.FromBytes(bytes);
        var audioClip = AudioClip.Create(clipName, pcmData.Length, pcmData.Channels, pcmData.SampleRate, false);
        audioClip.SetData(pcmData.Value, 0);
        return audioClip;
    }

    void OnApplicationQuit()
    {
        listener.Stop();
        client.Close();
    }

    void OnSwitch()
    {
        if (PlayerPrefs.GetString("Serverreceive") == "ATAvatar")
        {

            Debug.Log("re_Switch ATAvatar");
            listener.Stop();
            client.Close();
        }
        if (PlayerPrefs.GetString("Serverreceive") == "EndSession")
        {
            Debug.Log("re_Switch EndSession");
            listener.Stop();
            client.Close();
        }
    }
}
