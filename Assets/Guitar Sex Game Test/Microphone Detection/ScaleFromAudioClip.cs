using System;
using UnityEngine;

public class ScaleFromAudioClip : MonoBehaviour
{
    public AudioSource audioSource;
    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioLoudnessDetection detector;

    public float loudnessSensibility = 100;
    public float loudnessThreshold = 0.1f;
    private void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone();
        loudness *= loudnessSensibility;

        if (loudness < loudnessThreshold)
        {
            loudness = 0;
        }

        if (loudness != 0)
        {
            //Debug.Log(loudness);
        }
        
        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
        
    }
}
