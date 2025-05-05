using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SensitivityLevelEvent
{
    public string SensitivityLevel;

    public SensitivityLevelEvent(string sensitivityLevel)
    {
        SensitivityLevel = sensitivityLevel;
    }
}

public class SensitivityChangeEvent
{
    public float ChangeAmount;

    public SensitivityChangeEvent(float changeAmount)
    {
        ChangeAmount = changeAmount;
    }
}
public class SensitivityMeter : MonoBehaviour
{
    // if lower than this, an itchy signal will be evoked
    public float lowSensitivityThreshold = 30f;
    public float highSensitivityThreshold = 70f;
    
    public const float MeterCapacity = 100f;
    public float decreaseSpeed = 1f;
    
    private float _meterCapacity;

    private void Awake()
    {
        _meterCapacity = MeterCapacity;
        EventBetter.Listen(this, (SensitivityChangeEvent gse) =>
        {
            _meterCapacity += gse.ChangeAmount;
        });
    }

    private void Update()
    {
        if (_meterCapacity > MeterCapacity)
        {
            _meterCapacity = MeterCapacity;
        }
        if (_meterCapacity > 0)
        {
            _meterCapacity -= decreaseSpeed * Time.deltaTime;
        }
        
        if (_meterCapacity < lowSensitivityThreshold)
        {
            EventBetter.Raise(SensitivityLevel.LowSensitivity);
        }
        else if(_meterCapacity > lowSensitivityThreshold && _meterCapacity < highSensitivityThreshold)
        {
            EventBetter.Raise(SensitivityLevel.PerfectSensitivity);
        }
        else if (_meterCapacity > highSensitivityThreshold)
        {
            EventBetter.Raise(SensitivityLevel.Tired);
        }
        
    }
}
