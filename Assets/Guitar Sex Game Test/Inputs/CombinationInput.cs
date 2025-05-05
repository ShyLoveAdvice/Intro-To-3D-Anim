using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

// This class is used to combine inputs from guitar and makey makey and listen to alien needs events
public class CombinationInput : MonoBehaviour
{
    // Guitar Inputs
    public LoudnessRemap loudnessRemap;
    public float loudness;
    
    // MM Inputs
    public bool topLeftButtonPressing;
    public bool topRightButtonPressing;
    public bool bottomLeftButtonPressing;
    public bool bottomRightButtonPressing;
    
    // Needs
    [Header("Needs")]
    // if the player accomplish the needs with loudness over the percentage of it and mminputs holding more than that percentage, the need is met
    [Range(0, 1)] public float minMeetNeedsPercentage;
    private float _needsResponseTimer;

    private void Awake()
    {
        EventBetter.Listen(this, (MMInputs mmi) => {
            switch (mmi)
            {
                case MMInputs.ButtonTopLeft:
                    topLeftButtonPressing = true;
                    break;
                case MMInputs.ButtonTopRight:
                    topRightButtonPressing = true;
                    break;
                case MMInputs.ButtonBottomLeft:
                    bottomLeftButtonPressing = true;
                    break;
                case MMInputs.ButtonBottomRight:
                    bottomRightButtonPressing = true;
                    break;
            }
        });
        
        EventBetter.Listen(this, (RemappedLoudnessEvent rle) => { loudness = rle.RemappedLoudness;});
        
        EventBetter.Listen(this, (NeedEvent ne) =>
        {
            StartCoroutine(ResponseRoutine(ne));
        });
    }

    private IEnumerator ResponseRoutine(NeedEvent ne)
    {
        _needsResponseTimer = ne.NeedReponseInterval;
        float metNeedTimer = 0f;
        while (_needsResponseTimer > 0)
        {
            _needsResponseTimer -= Time.deltaTime;
            
            // if loudness met
            if (loudness >= ne.MinLoudnessNeed && loudness <= ne.MaxLoudnessNeed)
            {
                // if mminputs met
                bool mminputsMet = false;
                switch (ne.MmInputsNeed)
                {
                    case MMInputs.ButtonTopLeft:
                        if (topLeftButtonPressing)
                        {
                            mminputsMet = true;
                        }
                        break;
                    case MMInputs.ButtonTopRight:
                        if (topRightButtonPressing)
                        {
                            mminputsMet = true;
                        }
                        break;
                    case MMInputs.ButtonBottomLeft:
                        if (bottomLeftButtonPressing)
                        {
                            mminputsMet = true;
                        }
                        break;
                    case MMInputs.ButtonBottomRight:
                        if (bottomRightButtonPressing)
                        {
                            mminputsMet = true;
                        }
                        break;
                }

                if (mminputsMet)
                {
                    metNeedTimer += Time.deltaTime;
                }
            }
            DebugOnScreen.Set("Met Need Progression: ", $"Time Left: {_needsResponseTimer}\nPercentage: {metNeedTimer / ne.NeedReponseInterval}");
            yield return null;
        }
        

        if ((metNeedTimer / ne.NeedReponseInterval) > minMeetNeedsPercentage)
        {
            EventBetter.Raise(new SensitivityChangeEvent(ne.NeedMetChangeAmount));
        }
        else
        {
            EventBetter.Raise(new SensitivityChangeEvent(ne.NeedNotMetChangeAmount));
        }
    }
    private void LateUpdate()
    {
        topLeftButtonPressing = false; topRightButtonPressing = false; bottomLeftButtonPressing = false; bottomRightButtonPressing = false;
    }
}
