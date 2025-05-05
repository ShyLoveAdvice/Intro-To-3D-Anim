using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum SensitivityLevel
{
    LowSensitivity,
    PerfectSensitivity,
    Tired
}

public class NeedEvent
{
    public float MinLoudnessNeed;
    public float MaxLoudnessNeed;
    public MMInputs MmInputsNeed;
    public float NeedReponseInterval;
    public float NeedMetChangeAmount;
    public float NeedNotMetChangeAmount;

    public NeedEvent(float minLoudnessNeed, float maxLoudnessNeed, MMInputs mmInputsNeed, float needResponseInterval,
        float needMetChangeAmount, float needNotMetChangeAmount)
    {
        MinLoudnessNeed = minLoudnessNeed;
        MaxLoudnessNeed = maxLoudnessNeed;
        MmInputsNeed = mmInputsNeed;
        NeedReponseInterval = needResponseInterval;
        NeedMetChangeAmount = needMetChangeAmount;
        NeedNotMetChangeAmount = needNotMetChangeAmount;
    }
}

public class AlienNeeds : MonoBehaviour
{
    public float needNotMetChangeAmount;

    [Header("Low Sensitivity")] public float minLoudnessNeedAtLowSensitivity;
    public float maxLoudnessNeedAtLowSensitivity;

    public float needMetSensitivityChangeAtLowSensitivity;
    [Header("Perfect Sensitivity")] public float minLoudnessNeedAtPerfectSensitivity;
    public float maxLoudnessNeedAtPerfectSensitivity;

    public float needMetSensitivityChangeAtPerfectSensitivity;
    [Header("Tired")] public float minLoudnessTiredSensitivity;
    public float maxLoudnessTiredSensitivity;

    public float needMetSensitivityChangeAtTired;

    public SensitivityLevel sensitivityLevel;
    public float needsMinInterval;
    public float needsMaxInterval;
    private float _needsInterval;

    private void Awake()
    {
        EventBetter.Listen(this, (SensitivityLevel sl) => { sensitivityLevel = sl; });
    }

    private void Update()
    {
        // Send current sensitivity events
        if (_needsInterval <= 0)
        {
            _needsInterval = Random.Range(needsMinInterval, needsMaxInterval);
            float minCurrentLoudnessNeed = 0f;
            float maxCurrentLoudnessNeed = 0f;
            float needMetChangeAmount = 0f;
            switch (sensitivityLevel)
            {
                case SensitivityLevel.LowSensitivity:
                    minCurrentLoudnessNeed = minLoudnessNeedAtLowSensitivity;
                    maxCurrentLoudnessNeed = maxLoudnessNeedAtLowSensitivity;
                    needMetChangeAmount = needMetSensitivityChangeAtLowSensitivity;
                    break;
                case SensitivityLevel.PerfectSensitivity:
                    minCurrentLoudnessNeed = minLoudnessNeedAtPerfectSensitivity;
                    minCurrentLoudnessNeed = maxLoudnessNeedAtPerfectSensitivity;
                    needMetChangeAmount = needMetSensitivityChangeAtPerfectSensitivity;
                    break;
                case SensitivityLevel.Tired:
                    minCurrentLoudnessNeed = minLoudnessTiredSensitivity;
                    minCurrentLoudnessNeed = maxLoudnessTiredSensitivity;
                    needMetChangeAmount = needMetSensitivityChangeAtTired;
                    break;
            }

            // this needs event is listened by combination inputs
            var currentNeed = new NeedEvent(minCurrentLoudnessNeed, maxCurrentLoudnessNeed,
                Enum.GetValues(typeof(MMInputs)).Cast<MMInputs>().ToList()[
                    Random.Range(0, Enum.GetValues(typeof(MMInputs)).Length)], _needsInterval, needMetChangeAmount,
                needNotMetChangeAmount);
            EventBetter.Raise(currentNeed);

            DebugOnScreen.Set("Alien Need Event:",
                $"Loudness Required: (min): {currentNeed.MinLoudnessNeed}, (max): {currentNeed.MaxLoudnessNeed}\n" +
                $"MMInput Required: {currentNeed.MmInputsNeed}");
        }

        _needsInterval -= Time.deltaTime;
    }
}