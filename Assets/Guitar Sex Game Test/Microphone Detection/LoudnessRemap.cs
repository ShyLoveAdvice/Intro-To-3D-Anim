using UnityEngine;

public class RemappedLoudnessEvent
{
    public float RemappedLoudness;

    public RemappedLoudnessEvent(float remappedLoudness)
    {
        RemappedLoudness = remappedLoudness;
    }
}
public class LoudnessRemap : MonoBehaviour
{
    public float loudnessAt0;
    public float loudnessAt1;
    [SerializeField] private float _lerpedLoudness;
    
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
        
        _lerpedLoudness = Mathf.Lerp(loudnessAt0, loudnessAt1, loudness);
        
        EventBetter.Raise(new RemappedLoudnessEvent(_lerpedLoudness));
    }
}
