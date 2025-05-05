using System;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;

    public bool DebugUsingNativeMic = false;

    private void Start()
    {
        MicrophoneToAudioClip();
    }
    public void MicrophoneToAudioClip()
    {
        string micName = Microphone.devices[0];
        Debug.Log(micName);
        microphoneClip = Microphone.Start(micName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }
    private float GetLoudnessFromAudioClip(int clipPosition, AudioClip audioClip)
    {
        int startPosition = clipPosition - sampleWindow;
        float[] waveData = new float[sampleWindow ];

        if (startPosition < 0)
            return 0;
        
        audioClip.GetData(waveData, startPosition);
        
        float totalLoudness = 0.0f;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        return totalLoudness / sampleWindow;
    }
}